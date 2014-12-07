using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WPFLight.Helpers;
using WPFLight.Resources;
using System.Windows.Media;

namespace System.Windows.Controls {
	public class DataGrid : Panel {
		public DataGrid () {
			this.Items = new DataGridItemColleciton ();
			this.Columns = new List<ColumnHeader> ();
			this.FontScale = 1;

			loadedItems = new List<DataGridItem> ();
		}

		#region Events

		public event LoadItemEventHandler LoadItem;

		#endregion

		#region Properties

		public List<ColumnHeader> 	    Columns 	    { get; private set; }

		public DataGridItemColleciton   Items           { get; private set; }

		public float                    VerticalOffset  { get; private set; }

		#endregion

		public override void Initialize () {
			gridHeader = new Grid ();
			gridHeader.HorizontalAlignment = HorizontalAlignment.Stretch;
			gridHeader.VerticalAlignment = VerticalAlignment.Top;
			gridHeader.Height = 50;
			gridHeader.Opacity = 1;
			gridHeader.Background = new SolidColorBrush (
				this.GraphicsDevice, new System.Windows.Media.Color (1, 1, 1f) * .1f);

			foreach (var c in this.Columns)
				gridHeader.ColumnDefinitions.Add (c);

			this.Children.Add (gridHeader);

			rcScrollBar = new System.Windows.Shapes.Rectangle ();
			rcScrollBar.Width = 3;
			rcScrollBar.Margin = new Thickness (0, 2, 2, 2);
			rcScrollBar.HorizontalAlignment = HorizontalAlignment.Left;
			rcScrollBar.Fill = new SolidColorBrush (this.GraphicsDevice, new System.Windows.Media.Color (1f, 1f, 1f) * .45f);
			rcScrollBar.Height = 100;
			rcScrollBar.Left = this.ActualWidth - 7;
			rcScrollBar.Visibility = Visibility.Visible;
			this.Children.Add (rcScrollBar);

			base.Initialize ();
		}

		public override void OnTouchDown (TouchLocation state) {
			if (!scrolling)
				startPos = (int)(state.Position.Y - this.VerticalOffset);

			base.OnTouchDown (state);
		}

		public override void OnTouchUp (TouchLocation state) {
			scrolling = false;
			if (!scrolling)
				base.OnTouchUp (state);
		}

		public override void OnTouchMove (TouchLocation state) {
			var offset = Math.Min (0, state.Position.Y - startPos);
			if (-offset > (contentHeight - (this.ActualHeight - 50)))
				offset = -(contentHeight - (this.ActualHeight - 50));

			this.VerticalOffset = (int)offset;
			base.OnTouchMove (state);
		}

		public override void Update (GameTime gameTime) {
			if (this.ActualHeight < contentHeight)
				this.rcScrollBar.Top = (int)52 + (-((this.VerticalOffset * (this.ActualHeight - 50)) / contentHeight));

			base.Update (gameTime);
		}

		public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			base.Draw (gameTime, batch, alpha, transform);
			if (this.IsVisible) {

				var absLeft = this.GetAbsoluteLeft ();
				var absTop = this.GetAbsoluteTop ();
				var left = absLeft;
				var top = absTop;
				var i = 0;

				/*
                this.GraphicsDevice.ScissorRectangle =
                    ScreenHelper.CheckScissorRect(
                        ScreenHelper.Project(
                            new Microsoft.Xna.Framework.Rectangle(
                                (int)left, (int)top + 50, (int)this.ActualWidth, (int)this.ActualHeight - 50)));

				*/

				var newLeft = absLeft;
				var newTop = absTop + 50;
				var newWidth = this.ActualWidth;
				var newHeight = this.ActualHeight - 50;

				var scaleX = (float)ScreenHelper.SCREEN_WIDTH / (float)ScreenHelper.ORIGINAL_WIDTH;
				var scaleY = (float)ScreenHelper.SCREEN_HEIGHT / (float)ScreenHelper.ORIGINAL_HEIGHT;

				var newBounds = 
					new Microsoft.Xna.Framework.Rectangle (
						(int)(newLeft * scaleX), (int)(newTop * scaleY), (int)(newWidth * scaleX), (int)(newHeight * scaleY));

				#if WINDOWS_PHONE
				if ( newBounds.Right > ScreenHelper.ORIGINAL_WIDTH )
					newBounds.Width = ScreenHelper.ORIGINAL_WIDTH - newBounds.Left;

				if ( newBounds.Bottom > ScreenHelper.ORIGINAL_HEIGHT )
					newBounds.Height = ScreenHelper.ORIGINAL_HEIGHT - newBounds.Top;
				#endif

				this.GraphicsDevice.ScissorRectangle = newBounds;

				foreach (var c in this.Columns) {
					var columnWidth = GetColumnWidth (i);
					if (!String.IsNullOrWhiteSpace (c.Text)) {
						batch.Begin (
							SpriteSortMode.Deferred,
							BlendState.AlphaBlend,
							null,
							DepthStencilState.None,
							RasterizerState.CullNone,
							null,
							transform);

						batch.DrawString (
							GetFont (),
							c.Text,
							new Vector2 (left + 10, top + 4),
							Microsoft.Xna.Framework.Color.White * .8f,
							0f,
							Vector2.Zero,
							this.FontScale,
							SpriteEffects.None,
							1f);

						batch.End ();
					}
					left += columnWidth;
					i++;
				}

				left = absLeft;
				top = absTop + this.VerticalOffset + 50;
				foreach (var item in this.Items) {
					if ((top + 30) >= absTop) {
						if (top > (absTop + this.ActualHeight))
							break;

						if (!loadedItems.Contains (item)) {
							if (this.LoadItem != null)
								LoadItem (this, new LoadItemEventArgs (item));

							loadedItems.Add (item);
						}

						left = absLeft;
						i = 0;
						foreach (var c in this.Columns) {
							var cellValue = item.Cells [i] ?? String.Empty;
							var columnWidth = this.GetColumnWidth (i);

							batch.Begin (
								SpriteSortMode.Deferred,
								BlendState.AlphaBlend,
								null,
								DepthStencilState.None,
								scissorEnabled,
								null,
								transform);

							batch.Draw (
								Textures.Background,
								new Microsoft.Xna.Framework.Rectangle (
									(int)(left + 2),
									(int)(top + 5),
									(int)(columnWidth - 1),
									(int)29),
								new Microsoft.Xna.Framework.Color (1f, 1f, 1f) * .08f);

							batch.End ();

							if (!String.IsNullOrWhiteSpace (c.Text)) {
								var itemLeft = left + 10;
								var itemWidth = cellValue != null ? (GetFont ().MeasureString (cellValue).X * FontScale * .9f) : 0;

								if (c.TextAlignment == TextAlignment.Right)
									itemLeft = left + columnWidth - itemWidth - 15;// + ( columnWidth - itemWidth - 10 );

								batch.Begin (
									SpriteSortMode.Deferred,
									BlendState.AlphaBlend,
									null,
									DepthStencilState.None,
									scissorEnabled,
									null,
									transform);

								batch.DrawString (
									GetFont (),
									cellValue,
									new Vector2 (itemLeft, top),
									c.TextColor,
									0f,
									Vector2.Zero,
									this.FontScale * .9f,
									SpriteEffects.None,
									1f);

								batch.End ();
							}
							left += columnWidth;
							i++;
						}
					}
					top += 30;
				}
			}
		}

		internal float GetStarColumnsWidth () {
			return
				this.ActualWidth
			- this.GetAutoColumnsWidth ()
			- this.GetAbsoluteColumnsWidth ();
		}

		internal float GetAutoColumnsWidth () {
			var result = 0f;
			var index = 0;
			foreach (var columnDef in this.Columns) {
				if (columnDef.Width.IsAuto)
					result += GetColumnWidth (index);

				index++;
			}
			return result;
		}

		internal float GetAbsoluteColumnsWidth () {
			var result = 0f;
			var index = 0;
			foreach (var columnDef in this.Columns) {
				if (columnDef.Width.IsAbsolute)
					result += GetColumnWidth (index);

				index++;
			}
			return result;
		}

		internal float GetColumnWidth (int column) {
			var result = 0f;
			if (this.Columns.Count > 0 && column < this.Columns.Count) {
				var def = this.Columns [column];
				if (def.Width.IsAbsolute)
					result = def.Width.Value;
				else if (def.Width.IsAuto) {
					/*
					if (this.Children.Count > 0) {
						var widths = this.Children.OfType<Control> ().Where (c => GetColumn ( c ) == column && c.Width != null);
						if (widths.Count () > 0)
							result = widths.Max (c => c.ActualWidth);
					}
					*/
				} else if (def.Width.IsStar) {
					if (def.Width.Value > 0) {
						var sum = this.Columns
							.Where (c => c.Width.IsStar)
								.Sum (c => c.Width.Value);

						result = (def.Width.Value / sum)
						* GetStarColumnsWidth ();
					} else
						result = 0;
				}
			} else
				result = this.ActualWidth;

			return result;
		}

		public void Refresh () {
			loadedItems.Clear ();
			contentHeight = this.Items.Count * 30f;
			rcScrollBar.Height = (float)(Math.Pow (this.ActualHeight - 50, 2) / contentHeight);
			if (rcScrollBar.Height < 20)
				rcScrollBar.Height = 20;

			this.rcScrollBar.Invalidate ();
			this.VerticalOffset = 0;
			offset = 0;
		}

		private bool scrolling;
		private float offset;
		private int startPos;
		private float contentHeight;
		private Grid gridHeader;
		private System.Windows.Shapes.Rectangle rcScrollBar;
		private List<DataGridItem> loadedItems;
		static RasterizerState scissorEnabled = 
			new RasterizerState { 
				CullMode = CullMode.None, 
				FillMode = FillMode.Solid, 
				ScissorTestEnable = true 
			};
	}
	public delegate void LoadItemEventHandler (object sender, LoadItemEventArgs e);
	public class LoadItemEventArgs : EventArgs {
		public LoadItemEventArgs (DataGridItem item) {
			this.Item = item;
		}

		public DataGridItem Item { get; set; }
	}
}

