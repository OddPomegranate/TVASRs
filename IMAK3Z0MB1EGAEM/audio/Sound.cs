using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using System.Net.Mime;
using System.Reflection.Metadata;

namespace IMAK3Z0MB1EGAEM.audio;

public class Sound
{
    private static Dictionary<string, SoundEffect> loadedSounds;

    public static void Init(ContentManager content)
    {
        loadedSounds = new Dictionary<string, SoundEffect>();
        loadedSounds["warpin"] = content.Load<SoundEffect>("sfx/warpin");
        loadedSounds["warpout"] = content.Load<SoundEffect>("sfx/warpout");
        loadedSounds["zexplode"] = content.Load<SoundEffect>("sfx/zexplode");
        loadedSounds["znormal"] = content.Load<SoundEffect>("sfx/znormal");
        loadedSounds["zrapid"] = content.Load<SoundEffect>("sfx/zrapid");
        loadedSounds["zrocked"] = content.Load<SoundEffect>("sfx/zrocked");
        loadedSounds["zspread"] = content.Load<SoundEffect>("sfx/zspread");
        loadedSounds["warpin"] = content.Load<SoundEffect>("sfx/warpin");
        loadedSounds["mexplode"] = content.Load<SoundEffect>("sfx/mexplode");
        loadedSounds["nekodie"] = content.Load<SoundEffect>("sfx/nekodie");
        loadedSounds["spit"] = content.Load<SoundEffect>("sfx/spit");
        loadedSounds["spitbomb"] = content.Load<SoundEffect>("sfx/spitbomb");
        loadedSounds["spitrapid"] = content.Load<SoundEffect>("sfx/spitrapid");
        loadedSounds["spitsplode"] = content.Load<SoundEffect>("sfx/spitsplode");
        loadedSounds["spitspread"] = content.Load<SoundEffect>("sfx/spitspread");
        loadedSounds["suit"] = content.Load<SoundEffect>("sfx/suit");
        loadedSounds["sword"] = content.Load<SoundEffect>("sfx/sword");
        loadedSounds["boot"] = content.Load<SoundEffect>("sfx/boot");
        loadedSounds["catzap"] = content.Load<SoundEffect>("sfx/catzap");
        loadedSounds["foot"] = content.Load<SoundEffect>("sfx/foot");
        loadedSounds["explode"] = content.Load<SoundEffect>("sfx/explode");
        loadedSounds["ibeam"] = content.Load<SoundEffect>("sfx/ibeam");
        loadedSounds["ihit"] = content.Load<SoundEffect>("sfx/ihit");
        loadedSounds["jail"] = content.Load<SoundEffect>("sfx/jail");
        loadedSounds["junk"] = content.Load<SoundEffect>("sfx/junk");
        loadedSounds["junkbit"] = content.Load<SoundEffect>("sfx/junkbit");
        loadedSounds["bomb"] = content.Load<SoundEffect>("sfx/bomb");
    }

	public static void Play(string s)
	{
		try
		{
            if (loadedSounds.TryGetValue(s, out SoundEffect soundEffect))
            {
                soundEffect.Play(0.2f, 0.0f, 0.0f);
            }

        }
		catch (Exception)
		{
		}
	}

	public static void Update()
	{
	}
}
