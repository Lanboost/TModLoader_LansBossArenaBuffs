using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LansBossArenaBuffs
{

	public class BossItemType
	{
		public string name;
		public int count;

		public BossItemType(string name, int count)
		{
			this.name = name;
			this.count = count;
		}
	}

	class LPlayer : ModPlayer
	{

		public override void PostUpdateBuffs()
		{
			base.PostUpdateBuffs();

			foreach(var entityPos in TileEntity.ByPosition)
			{
				if (entityPos.Value.type == ModContent.GetInstance<BossTileEntity1>().Type ||
					entityPos.Value.type == ModContent.GetInstance<BossTileEntity2>().Type ||
					entityPos.Value.type == ModContent.GetInstance<BossTileEntity3>().Type || 
					entityPos.Value.type == ModContent.GetInstance<BossTileEntity4>().Type)
				{

					var pos = entityPos.Key;
					var coord = Main.LocalPlayer.position.ToTileCoordinates16();

					var x = pos.X - coord.X + 5;
					var y = pos.Y - coord.Y + 5;

					float dist = (float)Math.Sqrt((double)(x * x + y * y));

					var bTile = (BossTileEntity) entityPos.Value;

					if (dist < 50)
					{
						bTile.adapter.ApplyBuffs(Main.LocalPlayer);
					}
					if (dist < 8)
					{
						bTile.adapter.ApplyHoneyBuffs(Main.LocalPlayer);
					}
				}
			}
		}
	}

	public class BossItem : ModItem
	{
		string tile;
		int index;
		public BossItem(string tile, int index)
		{
			this.tile = tile;
			this.index = index;
		}

		public override string Texture => (GetType().Namespace + "." + "BossItem"+ index).Replace('.', '/');

		public override bool CloneNewInstances => true;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a modded block.");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = false;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType(tile);
		}

		
	}



	public class LansBossArenaBuffs : Mod
	{

		public static LansBossArenaBuffs instance;
		

		public LansBossArenaBuffs()
		{
			instance = this;
			
		}

		public void add(string name, ModTileEntity tileEntity, int width, int height, string texture, int index)
		{
			this.AddTileEntity(name, tileEntity);


			this.AddTile(name, new BossTile(name, width, height, name), texture);

			AddItem(name, new BossItem(name, index));

		}

		public override void Load()
		{
			base.Load();


			this.add("Small camp", new BossTileEntity1(), 7, 4, "LansBossArenaBuffs/tile1", 1);

			this.add("Medium camp", new BossTileEntity2(), 7, 4, "LansBossArenaBuffs/tile2", 2);

			this.add("Large camp", new BossTileEntity3(), 9, 4, "LansBossArenaBuffs/tile3", 3);

			this.add("Large camp (2x Heart)", new BossTileEntity4(), 9, 4, "LansBossArenaBuffs/tile3", 3);

		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Campfire);
			recipe.AddIngredient(ItemID.Sunflower);
			recipe.AddIngredient(ItemID.StarinaBottle);
			recipe.AddIngredient(ItemID.HeartLantern);
			recipe.SetResult(this, "Small camp");
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this.GetItem("Small camp"));
			recipe.AddIngredient(ItemID.HoneyBucket);
			recipe.SetResult(this, "Medium camp");
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this.GetItem("Medium camp"));
			recipe.AddIngredient(ItemID.HeartStatue);
			recipe.SetResult(this, "Large camp");
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this.GetItem("Large camp"));
			recipe.AddIngredient(ItemID.HeartStatue);
			recipe.SetResult(this, "Large camp (2x Heart)");
			recipe.AddRecipe();



			/*

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this, "BossTile1");
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this, "BossTile2");
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this, "BossTile2");
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this, "BossTile3");
			recipe.AddRecipe();*/
		}
	}
}