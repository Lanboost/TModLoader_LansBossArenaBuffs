using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossArenaBuffs
{

	class BossItemType
	{
		string name;
		int count;

		public BossItemType(string name, int count)
		{
			this.name = name;
			this.count = count;
		}
	}

	class BossItemDef
	{
		BossItemType[] bossItemTypes;
	}

	class BossItem : ModItem
	{
		string tile;
		BossItemType[] bossItemTypes;
		public BossItem(string tile, BossItemType[] bossItemTypes)
		{
			this.tile = tile;
			this.bossItemTypes = bossItemTypes;
		}

		public override string Texture => (GetType().Namespace + "." + "BossItem").Replace('.', '/');

		public override bool CloneNewInstances => true;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a modded block.");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 1;
			item.useTurn = true;
			item.autoReuse = false;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType(tile);
		}

		
	}



	public class BossArenaBuffs : Mod
	{

		public static BossArenaBuffs instance;

		public List<BossItemDef> items = new List<BossItemDef>();

		public BossArenaBuffs()
		{
			instance = this;



		}

		public override void Load()
		{
			base.Load();


			AddItemToTiles(new BossItemType("Heart lantern", 1));
			AddItemToTiles(new BossItemType("Sun Flower", 1));
			AddItemToTiles(new BossItemType("Honey Bucket", 1));
			AddItemToTiles(new BossItemType("Campfire", 1));

			for (int i = 0; i < 5; i++)
			{
				AddItemToTiles(new BossItemType("Heart statue", 1));
			}

			for (int i = 0; i < 5; i++)
			{
				AddItemToTiles(new BossItemType("Star statue", 1));
			}

			AddItems();

			AddItem("BossTile1", new BossItem("BossTile"));
			AddItem("BossTile2", new BossItem("BossTile"));
			AddItem("BossTile3", new BossItem("BossTile"));

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this, "BossTile1");
			recipe.AddRecipe();


			recipe = new ModRecipe(this);
			recipe.AddIngredient(this, "BossTile1");
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this, "BossTile2");
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this, "BossTile2");
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this, "BossTile3");
			recipe.AddRecipe();
		}
	}
}