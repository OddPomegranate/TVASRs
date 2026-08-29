using Microsoft.Xna.Framework;
using Viking_x86.director;

namespace Viking_x86.vikinggame;

public class SpawnMgr
{
	private Vector2 galagaVec;

	private float galagaFrame;

	private int galagaFace;

	public void SpawnGalaga()
	{
		if (!(galagaFrame > 0f))
		{
			galagaVec = Game1.vgame.charMgr.character[0].loc;
			int activeCount = 0;
			for (int i = 0; i < VikingGame.mainPlayerIdx.Length; i++)
			{
				if (Game1.vgame.charMgr.character[i].exists)
				{
					activeCount++;
				}
			}
			if (activeCount > 0)
			{
				int pick = Rand.GetRandomInt(0, activeCount - 1);
				int seen = 0;
				for (int i = 0; i < VikingGame.mainPlayerIdx.Length; i++)
				{
					if (!Game1.vgame.charMgr.character[i].exists)
					{
						continue;
					}
					if (seen == pick)
					{
						galagaVec = Game1.vgame.charMgr.character[i].loc;
						break;
					}
					seen++;
				}
			}
			galagaVec.X = Game1.vgame.world.towerX + 160f;
			galagaVec.Y -= 300f;
			float num = 380f;
			if (Rand.CoinToss(0.5f))
			{
				galagaVec.X += num;
				galagaFace = 0;
			}
			else
			{
				galagaVec.X -= num;
				galagaFace = 1;
			}
			galagaFrame = 3f;
		}
	}

	public void Update()
	{
		if (galagaFrame > 0f)
		{
			float num = galagaFrame;
			galagaFrame -= Game1.frameTime;
			if ((int)(num * 5f) != (int)(galagaFrame * 5f))
			{
				Game1.vgame.charMgr.Init(6, galagaVec, default(Vector2), 0, galagaFace, 1);
			}
		}
	}

	public void Spawnemy(int type)
	{
		if (type == 5)
		{
			for (int i = 0; i < Game1.vgame.charMgr.character.Length; i++)
			{
				if (Game1.vgame.charMgr.character[i].exists && Game1.vgame.charMgr.character[i].defID == 5)
				{
					return;
				}
			}
		}
		if (type == 6)
		{
			SpawnGalaga();
			return;
		}
		Vector2 vector = default(Vector2);
		Vector2 loc = Game1.vgame.charMgr.character[0].loc;
		bool haveLoc = false;
		for (int p = 0; p < VikingGame.mainPlayerIdx.Length; p++)
		{
			if (Game1.vgame.charMgr.character[p].exists && (!haveLoc || Game1.vgame.charMgr.character[p].loc.Y < loc.Y))
			{
				loc = Game1.vgame.charMgr.character[p].loc;
				haveLoc = true;
			}
		}
		switch (type)
		{
		case 2:
		{
			loc += Rand.GetRandomVec2(-30f, 30f, -200f, 0f);
			float num = 400f;
			vector = Rand.GetRandomVec2(200f, 300f, -300f, -250f);
			if (Rand.CoinToss(0.5f))
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(0f - num, 0f), new Vector2(vector.X, vector.Y), 0, 1, 1);
			}
			else
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(num, 0f), new Vector2(0f - vector.X, vector.Y), 0, 0, 1);
			}
			break;
		}
		case 3:
		{
			loc += Rand.GetRandomVec2(-30f, 30f, -320f, -200f);
			float num = 300f;
			vector = new Vector2(200f, 0f);
			if (Rand.CoinToss(0.5f))
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(0f - num, 0f), new Vector2(vector.X, vector.Y), 0, 1, 1);
			}
			else
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(num, 0f), new Vector2(0f - vector.X, vector.Y), 0, 0, 1);
			}
			break;
		}
		case 7:
		case 8:
		{
			loc += Rand.GetRandomVec2(-30f, 30f, -320f, -200f);
			float num = 300f;
			vector = new Vector2(200f, 0f);
			if (Rand.CoinToss(0.5f))
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(0f - num, 0f), new Vector2(vector.X, vector.Y), 0, 1, 1);
			}
			else
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(num, 0f), new Vector2(0f - vector.X, vector.Y), 0, 0, 1);
			}
			break;
		}
		case 4:
			loc.X = Rand.GetRandomFloat(Game1.vgame.world.towerX, Game1.vgame.world.towerX + 320f);
			loc.Y -= 400f;
			vector.Y = 50f;
			Game1.vgame.charMgr.Init(type, loc, new Vector2(vector.X, vector.Y), 0, 1, 1);
			break;
		case 5:
		{
			loc.X = Game1.vgame.world.towerX + 160f;
			loc.Y += 300f;
			float num = 200f;
			vector = new Vector2(0f, 0f);
			if (Rand.CoinToss(0.5f))
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(0f - num, 0f), new Vector2(vector.X, vector.Y), 0, 1, 1);
			}
			else
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(num, 0f), new Vector2(0f - vector.X, vector.Y), 0, 0, 1);
			}
			break;
		}
		case 6:
		{
			loc += Rand.GetRandomVec2(-30f, 30f, -320f, -200f);
			float num = 300f;
			vector = new Vector2(200f, 0f);
			if (Rand.CoinToss(0.5f))
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(0f - num, 0f), new Vector2(vector.X, vector.Y), 0, 1, 1);
			}
			else
			{
				Game1.vgame.charMgr.Init(type, loc + new Vector2(num, 0f), new Vector2(0f - vector.X, vector.Y), 0, 0, 1);
			}
			break;
		}
		}
	}

	internal void SpawnPickup()
	{
		switch (TimeMgr.VikingTMgr().phase)
		{
		case 5:
			if (!Game1.vgame.charMgr.moon.active)
			{
				return;
			}
			break;
		case 6:
			return;
		}
		Game1.vgame.pMgr.AddParticle(15, Game1.vgame.world.risingBaseVec + Rand.GetRandomVec2(-160f, 160f, -600f, -600f), new Vector2(0f, 50f), 0f, Rand.GetRandomInt(0, 5), 0);
	}
}
