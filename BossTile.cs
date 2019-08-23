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

namespace BossArenaBuffs
{
	

	class BossTile:ModTile
	{

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Width = 11; // because the template is for 1x2 not 1x3
			TileObjectData.newTile.Height = 4; // because the template is for 1x2 not 1x3
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 }; // because height change
			// We set processedCoordinates to true so our Hook_AfterPlacement gets top left coordinates, regardless of Origin.
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<BossTileEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.addTile(Type);


			dustType = mod.DustType("Sparkle");
			drop = mod.ItemType("BossItem1");
			AddMapEntry(new Color(200, 200, 200));
			// Set other values here
		}

		

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return;
			base.PostDraw(i, j, spriteBatch);
			
			Texture2D texture = Main.tileTexture[TileID.Statues];
			if(texture == null)
			{
				Main.instance.LoadTiles(TileID.Statues);
				texture = Main.tileTexture[TileID.Statues];
			}


			var dpos = new Vector2(i + 3.5f, j+3.5f).ToWorldCoordinates()-Main.screenPosition;

			spriteBatch.Draw(texture, dpos, new Rectangle(74*18,0, 16, 16), Color.White);
			spriteBatch.Draw(texture, dpos+new Vector2(16, 0), new Rectangle(75 * 18, 0, 16, 16), Color.White);
			spriteBatch.Draw(texture, dpos + new Vector2(0, 16), new Rectangle(74 * 18, 18, 16, 16), Color.White);
			spriteBatch.Draw(texture, dpos + new Vector2(16, 16), new Rectangle(75 * 18, 18, 16, 16), Color.White);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			mod.GetTileEntity<BossTileEntity>().Kill(i, j);
			base.KillMultiTile(i, j, frameX, frameY);
			Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("BossTile1"));
		}
		
	}

	class BossTileEntity: ModTileEntity
	{
		int time = 0;
		int startime = 0;


		int hearts = 2;
		int stars = 5;


		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];


			if(tile.active())
			{

			}


			return tile.active() && tile.type == mod.TileType<BossTile>();
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			return Place(i, j);
		}

		public void ApplyBuffs(Player player)
		{

			Main.campfire = true;
			player.AddBuff(BuffID.Campfire, 2, false);

			Main.starInBottle = true;
			player.AddBuff(BuffID.StarInBottle, 2, false);

			Main.heartLantern = true;
			player.AddBuff(BuffID.HeartLamp, 2, false);

			Main.sunflower = true;
			player.AddBuff(BuffID.Sunflower, 2, false);
		}

		public void ApplyHoneyBuffs(Player player)
		{
			player.AddBuff(BuffID.Honey, 1800, true);
			player.honeyWet = true;
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

		public override void Update()
		{
			if(Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer)
			{
				this.time--;
				if (this.time <= 0)
				{
					var pos = this.Position.ToWorldCoordinates() + new Vector2(1*16, 2 * 16);

					if (MechSpawn(pos.X, pos.Y, ItemID.Heart) < 3* hearts)
					{
						Item.NewItem(pos, new Vector2(0, 0), ItemID.Heart);
					}
					time = 600/ hearts;
				}

				this.startime--;
				if (this.startime <= 0)
				{
					var pos = this.Position.ToWorldCoordinates()+ new Vector2(10f*16, 2*16);

					if (MechSpawn(pos.X, pos.Y, ItemID.Star) < 3*stars)
					{
						Item.NewItem(pos, new Vector2(0, 0), ItemID.Star);
					}
					startime = 600 / stars;
				}
			}


			if(Main.netMode != NetmodeID.Server)
			{
				var pos = this.Position;
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
			base.Update();
		}
	}

	
	
}
