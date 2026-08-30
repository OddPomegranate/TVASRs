using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Viking_x86;

public class SpriteTools
{
	public static SpriteBatch sprite;

	// Global presentation transform: scales/letterboxes the fixed 1280x720
	// virtual playfield to fit whatever the actual window/backbuffer size is.
	// Every Begin() below routes through this so resizing/fullscreen doesn't
	// require touching any individual draw call anywhere else in the game.
	public static Matrix transform = Matrix.Identity;

	public static void BeginOpaque()
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, null, transform);
	}

	public static void BeginAlpha()
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, transform);
	}

	public static void BeginAlphaPoint()
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transform);
	}

	public static void BeginAdditivePoint()
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, transform);
	}

	public static void BeginAdditive()
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, transform);
	}

	public static void BeginAlpha(Effect effect)
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, effect, transform);
	}

	public static void BeginAdditive(Effect effect)
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, effect, transform);
	}

	public static void BeginOpaque(Effect effect)
	{
		sprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, effect, transform);
	}

	public static void End()
	{
		sprite.End();
	}
}
