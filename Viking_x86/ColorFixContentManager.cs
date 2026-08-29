using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Viking_x86;

// PORTING FIX: this project's Content/gfx/*.xnb files were re-compiled by MonoGame's
// own MGCB content builder for platform 'd' (DesktopGL) -- unlike the raw, never-
// rebuilt Xbox 360 content seen in the 20GPC project, these went through the real
// content pipeline. Even so, decoded pixels still come out with Green and Blue
// swapped (Red and Alpha are fine) -- confirmed empirically by dumping the actual
// decoded "raptor" texture, saving it as a PNG, and comparing it against a manual
// Green/Blue channel swap, which turned a magenta/purple raptor into a correct
// olive-green one with clean, correctly-shaded scale/stripe detail (not scrambled or
// noisy -- shape and shading were already right, only the hue was wrong). Best guess
// at the mechanism: whatever source art was extracted from the original Xbox 360
// assets to feed into MGCB already had this channel swap baked in, and MGCB just
// faithfully re-encoded it -- the compiler itself isn't at fault here.
//
// Same fix as 20GPC's ColorFixContentManager: a ContentManager subclass that swaps
// Green and Blue back for every uncompressed (SurfaceFormat.Color) Texture2D it
// loads, once, right after loading. Compressed formats (Dxt1/3/5) are left alone --
// no evidence they have this problem. Wire this into Game1.cs's base.Content instead
// of touching each individual Content.Load<Texture2D> call site.
//
// Known limitation: only corrects mip level 0 -- not an issue for anything checked
// so far, but worth knowing if a texture with real mipmaps ever needs this too.
internal class ColorFixContentManager : ContentManager
{
	private readonly HashSet<Texture2D> _fixedTextures = new HashSet<Texture2D>();

	// Assets whose Content/gfx/*.xnb pixels are already in correct, final color
	// order and must NOT have Green/Blue swapped - e.g. "gfx/grass", which was
	// rebuilt by hand from an already-correctly-colored source PNG instead of
	// going through the original (swap-needing) Xbox->PC conversion.
	private static readonly HashSet<string> _skipSwap = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"gfx/grass",
	};

	public ColorFixContentManager(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	public ColorFixContentManager(IServiceProvider serviceProvider, string rootDirectory)
		: base(serviceProvider, rootDirectory)
	{
	}

	public override T Load<T>(string assetName)
	{
		T asset = base.Load<T>(assetName);
		if (asset is Texture2D tex && tex.Format == SurfaceFormat.Color && !_fixedTextures.Contains(tex) && !_skipSwap.Contains(assetName))
		{
			SwapGreenBlue(tex);
			_fixedTextures.Add(tex);
		}
		return asset;
	}

	private static void SwapGreenBlue(Texture2D tex)
	{
		Color[] pixels = new Color[tex.Width * tex.Height];
		tex.GetData(pixels);
		for (int i = 0; i < pixels.Length; i++)
		{
			Color p = pixels[i];
			pixels[i] = new Color((int)p.R, (int)p.B, (int)p.G, (int)p.A);
		}
		tex.SetData(pixels);
	}
}
