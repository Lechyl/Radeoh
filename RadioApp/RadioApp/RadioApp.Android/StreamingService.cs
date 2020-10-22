using Android.Media;
using RadioApp.Droid;
using Xamarin.Forms;
using RadioApp.Services;

[assembly: Dependency(typeof(StreamingService))]
namespace RadioApp.Droid
{
    public class StreamingService : IStreaming
    {
        public StreamingService()
        {

        }
        MediaPlayer player;
        public string DataSource { get; set; }

        bool isPrepared;

        public void Play()
        {
            if (!isPrepared)
            {
                if (player == null)
                    player = new MediaPlayer();
                else
                    player.Reset();

                player.SetDataSource(DataSource);
                player.PrepareAsync();
            }

            player.Prepared += (sender, args) =>
            {
                player.Start();
                isPrepared = true;
            };
        }

        public void Stop()
        {
            player.Stop();
            isPrepared = false;
        }
    }
}