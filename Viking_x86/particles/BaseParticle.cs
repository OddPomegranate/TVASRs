using Microsoft.Xna.Framework;

namespace Viking_x86.particles;

public class BaseParticle
{
	public virtual void Init(Particle p, Vector2 loc, Vector2 traj, float size, int flags, int owner)
	{
		p.exists = true;
	}

	public virtual void Update(Particle p)
	{
		p.loc += p.traj * Game1.frameTime;
		p.frame -= Game1.frameTime;
		if (p.frame < 0f)
		{
			p.exists = false;
		}
	}

	public virtual void Draw(Particle p)
	{
	}

	// Tints the raptor "spit" projectile family (Spit/SpitBomb/SpitDrip/Spitnel)
	// to match the shooter's raptor recolor: owners 0-1 (Players 1-2) keep the
	// original green spit, owner 2 (Player 3) spits orange/salmon, owner 3
	// (Player 4) spits magenta/pink - matching their raptor sprite hues.
	protected static Color GetSpitTint(int owner, float r)
	{
		switch (owner)
		{
		case 2:
			return new Color(1f, 0.3f + r * 0.3f, 0.15f, 0.5f);
		case 3:
			return new Color(0.9f, 0.1f + r * 0.3f, 0.9f, 0.5f);
		default:
			return new Color(r, 1f, 0.2f, 0.5f);
		}
	}
}
