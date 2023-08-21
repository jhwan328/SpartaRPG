using Newtonsoft.Json;
using SpartaRPG.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaRPG.Managers.SceneManager;

namespace SpartaRPG.Managers
{
    internal class SceneManager
    {
        public GameManager GameManager;

        private string _path = @"../../../Scenes";
        public enum Scenes
        {
            GAME_INTRO,
            GAME_OUTRO,
            STATUS,
            INVENTORY_MAIN,
            INVENTORY_EQUIP,
            INVENTORY_SORT,
            SHOP_MAIN,
            SHOP_BUY,
            SHOP_SELL,
            DUNGEON,
            SHELTER
        }

        public Scenes Scene;

        private int maxOption;

        public SceneManager()
        {
            Scene = Scenes.GAME_INTRO;
        }

        public void LoadScene()
        {
            string path = MakePath();

            if(path != string.Empty)
            {
                //if(File.Exists(path)) { return; }

                dynamic sceneData = JsonConvert.DeserializeObject(File.ReadAllText(path));
                UIManager ui = GameManager.Instance.UIManager;

                ui.PrintTitle(sceneData["Title"].ToString());
                ui.PrintDescription(sceneData["Description"].ToString());

                int scene = (int)Scene;

                if(scene >= 3 && scene <= 8)
                {
                    ui.PrintItemCategories();

                    if(scene >= 6 && scene <= 8)
                    {
                        ui.PrintGold();
                        if (scene == 6) ui.PrintShopItems(false);
                        else if (scene == 7) ui.PrintShopItems(true);
                        else ui.PrintInventoryItems(true);
                    }
                    else
                    {
                        if(scene == 4) ui.PrintInventoryItems(true);
                        else ui.PrintInventoryItems(false);
                    }
                }

                List<String> option = sceneData["Option"].ToObject(typeof(List<String>));
                ui.PrintOption(option);

                maxOption = option.Count;

                int input = CheckValidInput(0, maxOption);
            }
        }

        private string MakePath()
        {
            switch (Scene)
            {
                case Scenes.GAME_INTRO:
                    return Path.Combine(_path, "Town.json");
                case Scenes.STATUS:
                    return Path.Combine(_path, "Status.json");
                case Scenes.INVENTORY_MAIN:
                case Scenes.INVENTORY_EQUIP:
                case Scenes.INVENTORY_SORT:
                    return Path.Combine(_path, "Inventory.json");
                case Scenes.SHOP_MAIN:
                case Scenes.SHOP_BUY:
                case Scenes.SHOP_SELL:
                    return Path.Combine(_path, "Shop.json");
                case Scenes.DUNGEON:
                    return Path.Combine(_path, "Dungeon.json");
                case Scenes.SHELTER:
                    return Path.Combine(_path, "Shelter.json");
            }

            return string.Empty;
        }

        private int CheckValidInput(int min, int max)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        private void InputLoopForShop(bool sellMode)
        {
            DataManager dm = GameManager.Instance.DataManager;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret == 0) break;
                    else if (ret > 0)
                    {
                        // (구매 모드)
                        if (!sellMode && ret <= dm.Shop.Count)
                        {
                            // 선택한 장비
                            var selectedItem = dm.Shop[ret - 1];
                            //가 이미 구매됐다면
                            if (selectedItem.IsSold)
                                Console.WriteLine("이미 구매한 아이템입니다.");
                            //가 구매 가능하다면
                            else
                            {
                                // 돈이 충분하다면
                                if (dm.Player.Gold >= selectedItem.Price)
                                {
                                    Console.WriteLine("구매를 완료했습니다.");
                                    dm.BuyItem(selectedItem);
                                }
                                // 돈이 충분치 않다면
                                else
                                    Console.WriteLine("소지금이 부족합니다.");
                            }
                            continue;
                        }
                        // (판매 모드)
                        else if (sellMode && ret <= dm.Inventory.Count)
                        {
                            // 선택한 장비
                            var selectedItem = dm.Inventory[ret - 1];

                            // 가 장착 중이라면
                            if (selectedItem.IsEquipped)
                            {
                                Console.WriteLine("장착 중인 장비는 판매할 수 없습니다.");
                                Console.WriteLine("해제하고 판매하시겠습니까? (예: 0 / 아니오: 1)");
                                if (CheckValidInput(0, 1) == 0)
                                {
                                    dm.Unwear(selectedItem.Part);
                                    Console.WriteLine("판매를 완료했습니다.");
                                    dm.SellItem(selectedItem);
                                }
                            }
                            // 가 장착 중이 아니라면
                            else
                            {
                                Console.WriteLine("판매를 완료했습니다.");
                                dm.SellItem(selectedItem);
                            }
                            continue;
                        }
                    }

                }

                //if (input == "[")
                //{
                //    if (_category == null)
                //        _category = (Item.Parts)(Enum.GetValues(typeof(Item.Parts)).Length - 1);
                //    else if (_category == (Item.Parts)0)
                //        _category = null;
                //    else _category--;
                //}
                //else if (input == "]")
                //{
                //    if (_category == null)
                //        _category = (Item.Parts)0;
                //    else if (_category == (Item.Parts)(Enum.GetValues(typeof(Item.Parts)).Length - 1))
                //        _category = null;
                //    else _category++;
                //}
                //else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}
