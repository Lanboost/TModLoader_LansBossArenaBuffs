using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace LansBossArenaBuffs
{


	public class BossTile:ModTile
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			return false;
		}

		
		string itemDrop;
		string tileEntity;
		int sWidth;
		int sHeight;
		public BossTile(string itemDrop, int width, int height, string tileEntity)
		{
			this.itemDrop = itemDrop;
			this.sWidth = width;
			this.sHeight = height;
			this.tileEntity = tileEntity;
		}

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Width = sWidth; // because the template is for 1x2 not 1x3
			TileObjectData.newTile.Height = sHeight; // because the template is for 1x2 not 1x3

			int[] heights = new int[sHeight];
			for(int i=0; i< heights.Length; i++)
			{
				heights[i] = 16;
			}
			heights[heights.Length - 1] = 18;

			TileObjectData.newTile.CoordinateHeights = heights; // because height change
			// We set processedCoordinates to true so our Hook_AfterPlacement gets top left coordinates, regardless of Origin.
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity(tileEntity).Hook_AfterPlacement, -1, 0, true);
			TileObjectData.addTile(Type);


			dustType = mod.DustType("Sparkle");
			//drop = mod.ItemType(itemDrop);
			AddMapEntry(new Color(200, 200, 200));
			// Set other values here
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			mod.GetTileEntity(tileEntity).Kill(i, j);
			Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType(itemDrop));
		}
		
	}



	public class BossTileEntity1:ModTileEntity
	{
		BossTileEntityAdapter adapter;
		public BossTileEntity1()
		{
			adapter = new BossTileEntityAdapter(new BossItemType[] {
				new BossItemType("campfire", 1),
				new BossItemType("sunflower", 1),
				new BossItemType("heartlantern", 1),
				new BossItemType("starinabottle", 1),
			});
		}

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			return Place(i, j);
		}

		public override bool ValidTile(int i, int j)
		{
			return adapter.ValidTile(i, j, ModContent.TileType<BossTile>());
		}

		public override void Update()
		{
			adapter.Update(Position);
		}
	}

	public class BossTileEntity2 : ModTileEntity
	{
		BossTileEntityAdapter adapter;
		public BossTileEntity2()
		{
			adapter = new BossTileEntityAdapter(new BossItemType[] {
				new BossItemType("campfire", 1),
				new BossItemType("sunflower", 1),
				new BossItemType("heartlantern", 1),
				new BossItemType("starinabottle", 1),
				new BossItemType("honey", 1),
			});
		}

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			return Place(i, j);
		}

		public override bool ValidTile(int i, int j)
		{
			return adapter.ValidTile(i, j, ModContent.TileType<BossTile>());
		}

		public override void Update()
		{
			adapter.Update(Position);
		}
	}

	public class BossTileEntity3 : ModTileEntity
	{
		BossTileEntityAdapter adapter;
		public BossTileEntity3()
		{
			adapter = new BossTileEntityAdapter(new BossItemType[] {
				new BossItemType("campfire", 1),
				new BossItemType("sunflower", 1),
				new BossItemType("heartlantern", 1),
				new BossItemType("starinabottle", 1),
				new BossItemType("honey", 1),
				new BossItemType("heartstatue", 1),
			});
		}

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			return Place(i, j);
		}

		public override bool ValidTile(int i, int j)
		{
			return adapter.ValidTile(i, j, ModContent.TileType<BossTile>());
		}

		public override void Update()
		{
			adapter.Update(Position);
		}
	}

	public class BossTileEntity4 : ModTileEntity
	{
		BossTileEntityAdapter adapter;
		public BossTileEntity4()
		{
			adapter = new BossTileEntityAdapter(new BossItemType[] {
				new BossItemType("campfire", 1),
				new BossItemType("sunflower", 1),
				new BossItemType("heartlantern", 1),
				new BossItemType("starinabottle", 1),
				new BossItemType("honey", 1),
				new BossItemType("heartstatue", 2),
			});
		}

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			return Place(i, j);
		}

		public override bool ValidTile(int i, int j)
		{
			return adapter.ValidTile(i, j, ModContent.TileType<BossTile>());
		}

		public override void Update()
		{
			adapter.Update(Position);
		}
	}





	public class BossTileEntityAdapter
	{
		
		BossItemType[] bossItemCount;
		public BossTileEntityAdapter(BossItemType[] bossItemCount)
		{
			this.bossItemCount = bossItemCount;
		}


		int time = 0;
		int startime = 0;

		

		public bool Autoload(ref string name)
		{
			return false;
		}


		public bool ValidTile(int i, int j, int tileType)
		{
			Tile tile = Main.tile[i, j];

			if(tile.active())
			{

			}
			return tile.active() && tile.type == tileType && tile.frameX == 0 && tile.frameY == 0;
		}

		public void ApplyBuffs(Player player)
		{

			foreach(var bossItem in bossItemCount)
			{
				if(bossItem.name == "campfire")
				{
					Main.campfire = true;
					player.AddBuff(BuffID.Campfire, 2, false);
				}
				else if (bossItem.name == "starinabottle")
				{
					Main.starInBottle = true;
					player.AddBuff(BuffID.StarInBottle, 2, false);
				}
				else if (bossItem.name == "heartlantern")
				{
					Main.heartLantern = true;
					player.AddBuff(BuffID.HeartLamp, 2, false);
				}
				else if (bossItem.name == "sunflower")
				{
					Main.sunflower = true;
					player.AddBuff(BuffID.Sunflower, 2, false);
				}
			}
		}

		public void ApplyHoneyBuffs(Player player)
		{


			foreach (var bossItem in bossItemCount)
			{
				if (bossItem.name == "honey")
				{
					player.AddBuff(BuffID.Honey, 1800, true);
					player.honeyWet = true;
				}
			}
		}
		
		public int MechSpawn(float x, float y, int type)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.item[i].active && Main.item[i].type == type)
				{
					num++;
					Vector2 vector = new Vector2(x, y);
					float num4 = Main.item[i].position.X - vector.X;
					float num5 = Main.item[i].position.Y - vector.Y;
					float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
					if (num6 < 300f)
					{
						num2++;
					}
					if (num6 < 800f)
					{
						num3++;
					}
				}
			}
			return Math.Max(num2, num);
		}

		public int getHearts()
		{
			foreach (var bossItem in bossItemCount)
			{
				if (bossItem.name == "heartstatue")
				{
					return bossItem.count;
				}
			}
			return 0;
		}

		public int getStars()
		{
			foreach (var bossItem in bossItemCount)
			{
				if (bossItem.name == "starstatue")
				{
					return bossItem.count;
				}
			}
			return 0;
		}

		public void Update(Point16 position)
		{
			if(Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer)
			{
				int hearts = getHearts();
				if (hearts > 0)
				{
					this.time--;
					if (this.time <= 0)
					{
						var pos = position.ToWorldCoordinates() + new Vector2(1 * 16, 2 * 16);

						if (MechSpawn(pos.X, pos.Y, ItemID.Heart) < 3 * hearts)
						{
							Item.NewItem(pos, new Vector2(0, 0), ItemID.Heart);
						}
						time = 600 / hearts;
					}
				}
				int stars = getStars();
				if (stars > 0)
				{
					this.startime--;
					if (this.startime <= 0)
					{
						var pos = position.ToWorldCoordinates() + new Vector2(10f * 16, 2 * 16);

						if (MechSpawn(pos.X, pos.Y, ItemID.Star) < 3 * stars)
						{
							Item.NewItem(pos, new Vector2(0, 0), ItemID.Star);
						}
						startime = 600 / stars;
					}
				}
			}


			if(Main.netMode != NetmodeID.Server)
			{
				var pos = position;
				var coord = Main.LocalPlayer.position.ToTileCoordinates16();

				var x = pos.X - coord.X+5;
				var y = pos.Y - coord.Y+5;

				float dist = (float)Math.Sqrt((double)(x * x + y * y));

				if(dist < 50)
				{
					ApplyBuffs(Main.LocalPlayer);
				}
				if (dist < 8)
				{
					ApplyHoneyBuffs(Main.LocalPlayer);
				}
			}
		}
	}

	
	
}
