using System;
using Microsoft.Xna.Framework;

namespace System.Windows.Media.Animation
{
    public interface IAnimationBase
    {
        void Begin();
        void Begin(TimeSpan beginTime);
        void Pause();
        void Resume();
        void Stop();
        void Update(GameTime gameTime);

        TimeSpan BeginTime { get; set; }
    }
}
