using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Viking_x86.director;

namespace IMAK3Z0MB1EGAEM.audio;

internal class Music
{
	public const int ZOMBIE_SONG = 0;

	public const int VIKING_SONG = 1;

	public const int ENDLESS_SONG = 2;

	public static string[] SONG = new string[3] { "epicopus", "timeviking", "endless" };

	public static int song;

	private static Song songCue;

	// True once Prime() has kicked off silent playback ahead of time, so the
	// audio pipeline (OpenAL buffer priming / decoder startup) has already
	// paid its latency cost before we actually need audible sound.
	private static bool primed;

	// PlayPosition value captured at the moment gameplay-relevant playback
	// "officially" begins (Start() / the first real Update() after a Prime()).
	// Used to zero the beat clock at that instant even if MediaPlayer.Play()
	// was actually called earlier during priming, so the early/silent
	// pre-roll never shifts beat sync.
	private static double startOffset;

	// Manual fine-tune, in seconds, for any remaining audio/visual mismatch
	// after Prime()'s buffer warm-up. This does NOT change when the music is
	// actually heard (that's fixed by the audio hardware/driver) - it shifts
	// when the beat clock (spawns, flashes, anything driven by TimeMgr.time)
	// considers the song to have started, so gameplay can be re-synced to
	// match what you actually hear.
	//
	//   - If the music still sounds like it lags BEHIND the on-screen action
	//     (you see the beat happen, then hear it a moment later), INCREASE
	//     this value in small steps (try 0.05-0.1 at a time) - it delays the
	//     beat-driven gameplay to match the late audio.
	//   - If the music now sounds AHEAD of the action, use a NEGATIVE value
	//     instead - it makes beat-driven gameplay fire sooner.
	//
	// There's no universally correct number here, it depends on your audio
	// driver/hardware, so tune it by ear and rebuild.
	public static double SyncOffsetSeconds = -1;

	public static void Init(ContentManager Content)
	{
        songCue = Content.Load<Song>("sfx/music/timeviking");
	}

	// Kick off playback muted, ahead of when it's actually needed, so any
	// MonoGame/OpenAL startup latency (buffer priming, decoder spin-up)
	// happens silently during a loading/transition window instead of as an
	// audible gap once gameplay is visible. Call this shortly (e.g. the ~1
	// second warp-in) before you'd normally call Start()/Update().
	public static void Prime()
	{
		if (primed || MediaPlayer.State == MediaState.Playing)
		{
			return;
		}
		try
		{
			MediaPlayer.Play(songCue);
			MediaPlayer.Volume = 0f;
			primed = true;
		}
		catch
		{
		}
	}

	public static void Update(int _song)
	{
		song = _song;
		TimeMgr.time = _song;
		if (MediaPlayer.State != MediaState.Playing)
		{
			if (TimeMgr.CurTMgr().playNum <= 0)
			{
				MediaPlayer.Play(songCue);
				MediaPlayer.Volume = 2f;
				primed = false;
				startOffset = MediaPlayer.PlayPosition.TotalSeconds + SyncOffsetSeconds;
				TimeMgr.CurTMgr().Start();
			}
		}
		else if (primed && TimeMgr.CurTMgr().playNum <= 0)
		{
			MediaPlayer.Volume = 2f;
			primed = false;
			startOffset = MediaPlayer.PlayPosition.TotalSeconds + SyncOffsetSeconds;
			TimeMgr.CurTMgr().Start();
		}
		else if (MediaPlayer.PlayPosition.TotalSeconds - startOffset > 901.0)
		{
			MediaPlayer.Stop();
		}
		TimeMgr.CurTMgr().time = MediaPlayer.PlayPosition.TotalSeconds - startOffset;
	}

	public static void Stop()
	{
		try
		{
			MediaPlayer.Stop();
		}
		catch
		{
		}
		primed = false;
	}

	public static void Start()
	{
		try
		{
			MediaPlayer.Play(songCue);
			MediaPlayer.Volume = 2f;
			primed = false;
			startOffset = MediaPlayer.PlayPosition.TotalSeconds + SyncOffsetSeconds;
		}
		catch
		{
		}
		TimeMgr.CurTMgr().Start();
	}

	public static void Pause()
	{
		try
		{
			MediaPlayer.Pause();
		}
		catch
		{
		}
	}

	public static void Resume()
	{
		try
		{
			MediaPlayer.Resume();
		}
		catch
		{
		}
	}
}
