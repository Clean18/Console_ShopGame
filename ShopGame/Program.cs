using System;
using System.Collections.Generic;
using System.Reflection;

namespace ShopGame
{
    class Program
    {
        struct Item
        {
            public int id;
            public string name;
            public string description;
            public int price;
            public int attack;
            public int defense;
            public int hp;
        }

        struct Player
        {
            public int money;
            public int attack;
            public int defense;
            public int hp;
            public List<Item> inventory;
        }

        static public int currentMenu;
        static Player player = new Player();
        static List<Item> shopItems = new List<Item>();

        static void Main(string[] args)
        {
            // 상점에서는 다음 작업들이 가능하다.
            // 1. 아이템 구매
            // 2. 아이템 판매
            // 3. 아이템 확인

            // 아이템은 기본적으로 이름, 설명, 가격을 가지고 있으며,
            //무기는 공격력, 방어구는 방어력, 장신구는 체력을 상승시키는 수치를 가진다.

            // 아이템 구매 메뉴 선택시 상점이 소유하고 있는 아이템들 목록이 제공되고,
            // 구매하고자 하는 아이템을 선택시 구매를 진행한다. 단, 돈이 부족하다면 구매는 진행되지 않는다.
            // 구매가 완료되면 소유한 아이템에 구매한 아이템이 추가되며, 아이템에 의해 플레이어 능력이 상승한다.

            // 아이템 판매 메뉴 선택시 플레이어가 소유하고 있는 아이템들 목록이 제공되고,
            // 판매하고자 하는 아이템을 선택시 판매를 진행한다. 단, 소유한 아이템이 없다면 진행되지 않는다.
            // 판매가 완료되면 소유한 아이템에 판매한 아이템이 제거되며, 아이템에 의해 플레이어 능력이 하락한다.

            // 아이템 확인 메뉴 선택시 플레이어가 소유하고 있는 아이템들 목록이 제공되고,
            // 아이템들에 의해 상승한 플레이어 최종 능력치를 보여준다.
            // 플레이어는 최대 6개의 아이템을 소유할 수 있으며 빈칸은 보여주지 않는다.



            PlayerInit(ref player);
            ItemInit();
            Init();

            PrintShopMain();

            while (true)
            {
                Render(ref player);
            }
        }

        static void Init() // 게임 설정 초기화
        {
            currentMenu = 0;
        }

        static void PlayerInit(ref Player player) // 플레이어 초기화
        {
            player.money = 10000;
            player.attack = 0;
            player.defense = 0;
            player.defense = 0;
            player.inventory = new List<Item>();
        }

        static void ItemInit()
        {
            shopItems.Add(new Item
            {
                id = 1,
                name = "롱소드",
                description = "기본적인 검이다.",
                price = 450,
                attack = 15,
                defense = 0,
                hp = 0,
            });

            shopItems.Add(new Item
            {
                id = 2,
                name = "천갑옷",
                description = "얇은 갑옷이다.",
                price = 450,
                attack = 0,
                defense = 10,
                hp = 0,
            });

            shopItems.Add(new Item
            {
                id = 3,
                name = "여신의 눈물",
                description = "희미하게 푸른빛을 품고 있는 보석이다.",
                price = 800,
                attack = 0,
                defense = 0,
                hp = 300,
            });
        }

        static void Render(ref Player player)
        {
            switch (currentMenu)
            {
                case 0:
                    PrintShopMain();
                    break;
                case 1:
                    PrintShopBuy(ref player);
                    break;
                case 2:
                    PrintShopSell(ref player);
                    break;
                case 3:
                    PrintPlayerInfo(ref player);
                    break;
            }
        }

        static void PrintShopMain()
        {
            Console.Clear();
            Console.WriteLine("*******************************");
            Console.WriteLine("********* 아이템 상점 *********");
            Console.WriteLine("*******************************");
            Console.WriteLine();
            Console.WriteLine("========== 상점 메뉴 ==========");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("3. 아이템 확인");
            Console.Write("메뉴를 선택하세요 : ");

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (result == 1)
                {
                    currentMenu = 1;
                }
                else if (result == 2)
                {
                    currentMenu = 2;
                }
                else if (result == 3)
                {
                    currentMenu = 3;
                }
            }
        }

        static void PrintShopBuy(ref Player player)
        {
            string moneyString = GetMoneyString(player.money);

            Console.Clear();
            Console.WriteLine("========== 아이템 구매 ==========");
            Console.WriteLine($"보유한 골드 : {moneyString}");
            for (int i = 0; i < shopItems.Count; i++)
            {
                Console.WriteLine();
                PrintItemInfo(shopItems[i], i);
                Console.WriteLine();
            }
            Console.Write("구매할 아이템을 선택하세요 (취소 0) : ");

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (result == 0)
                {
                    currentMenu = 0;
                }
                else if (1 <= result && result <= shopItems.Count)
                {
                    ItemBuy(ref player, result);
                }
            }
        }

        static void PrintShopSell(ref Player player)
        {
            List<Item> inventory = player.inventory;

            Console.Clear();
            Console.WriteLine("========== 아이템 판매 ==========");

            if (inventory.Count == 0)
            {
                Console.WriteLine();
                Console.Write("판매할 아이템이 없습니다 (취소 0) : ");
            }
            else
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.WriteLine();
                    PrintItemInfo(inventory[i], i);
                    Console.WriteLine();
                }

                Console.Write("판매할 아이템을 선택하세요 (취소 0) : ");
            }

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (result == 0)
                {
                    currentMenu = 0;
                }
                else if (1 <= result && result <= inventory.Count)
                {
                    ItemSell(ref player, result);
                }
            }
        }

        static void PrintPlayerInfo(ref Player player)
        {
            List<Item> inventory = player.inventory;

            Console.Clear();
            Console.WriteLine("========== 아이템 확인 ==========");
            Console.WriteLine($"플레이어   골드 보유량 : {GetMoneyString(player.money)}");
            Console.WriteLine($"플레이어 공격력 상승량 : {player.attack}");
            Console.WriteLine($"플레이어 방어력 상승량 : {player.defense}");
            Console.WriteLine($"플레이어   체력 상승량 : {player.hp}");

            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine();
                PrintItemInfo(inventory[i], i);
                Console.WriteLine();
            }
            
            currentMenu = 0;
            Console.Write("계속하려면 아무키나 입력하세요 : ");
            Console.ReadKey(false);
        }

        static string GetMoneyString(int money)
        {
            return money.ToString("N0")+"G";

            //if (money <= 999)
            //{
            //    return money.ToString();
            //}
            //else
            //{
            //    string frontString = (money / 1000).ToString();
            //    string endString = (money % 1000).ToString().PadLeft(3, '0');

            //    return frontString + "," + endString;
            //}
        }

        static void PrintItemInfo(Item item, int index) // 아이템 번호에 맞는 출력
        {
            Console.WriteLine($"{index + 1}. {item.name}");
            Console.WriteLine($"   가격 : {item.price}G");
            Console.WriteLine($"   설명 : {item.description}");

            if (item.attack != 0)
                Console.WriteLine($"   공격력 상승 : {item.attack}");
            if (item.defense != 0)
                Console.WriteLine($"   방어력 상승 : {item.defense}");
            if (item.hp != 0)
                Console.WriteLine($"   체력 상승 : {item.hp}");
        }

        static void ItemBuy(ref Player player, int itemNumber)
        {
            if (player.inventory.Count >= 6)
            {
                Console.WriteLine();
                Console.WriteLine("인벤토리가 가득 찼습니다.");
                Console.WriteLine("아무 키나 눌러주세요...");
                Console.ReadKey(true);
                return;
            }

            Item item = shopItems[itemNumber - 1];

            if (item.price > player.money)
            {
                Console.WriteLine();
                Console.WriteLine("골드가 부족합니다.");
                Console.WriteLine();
                Console.WriteLine("아무 키나 눌러주세요...");
                Console.ReadKey(true);
            }
            else
            {
                player.money -= item.price;

                player.inventory.Add(item);

                player.attack += item.attack;
                player.defense += item.defense;
                player.hp += item.hp;

                switch (itemNumber)
                {
                    case 1:
                        Console.WriteLine();
                        Console.WriteLine($"{item.name}를 구매합니다.");
                        Console.WriteLine($"플레이어의 공격력이 {item.attack} 상승하여 {player.attack}이 됩니다.");
                        Console.WriteLine($"보유한 골드가 {item.price}G 감소하여 {GetMoneyString(player.money)}가 됩니다.");
                        break;

                    case 2:
                        Console.WriteLine();
                        Console.WriteLine($"{item.name}를 구매합니다.");
                        Console.WriteLine($"플레이어의 방어력이 {item.defense} 상승하여 {player.defense}이 됩니다.");
                        Console.WriteLine($"보유한 골드가 {item.price}G 감소하여 {GetMoneyString(player.money)}가 됩니다.");
                        break;

                    case 3:
                        Console.WriteLine();
                        Console.WriteLine($"{item.name}을 구매합니다.");
                        Console.WriteLine($"플레이어의 체력이 {item.hp} 상승하여 {player.hp}이 됩니다.");
                        Console.WriteLine($"보유한 골드가 {item.price}G 감소하여 {GetMoneyString(player.money)}가 됩니다.");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("아무 키나 눌러주세요...");
                Console.ReadKey(true);
            }
        }

        static void ItemSell(ref Player player, int invenNumber)
        {
            Item item = player.inventory[invenNumber - 1];

            player.money += item.price;

            player.inventory.RemoveAt(invenNumber - 1);

            player.attack -= item.attack;
            player.defense -= item.defense;
            player.hp -= item.hp;

            Console.WriteLine();
            Console.WriteLine($"{item.name}를 판매합니다.");
            if (item.attack > 0)
                Console.WriteLine($"플레이어의 공격력이 {item.attack} 감소하여 {player.attack}이 됩니다.");
            if (item.defense > 0)
                Console.WriteLine($"플레이어의 방어력이 {item.defense} 감소하여 {player.defense}이 됩니다.");
            if (item.hp > 0)
                Console.WriteLine($"플레이어의 체력이 {item.hp} 감소하여 {player.hp}이 됩니다.");
            Console.WriteLine($"보유한 골드가 {item.price}G 상승하여 {GetMoneyString(player.money)}가 됩니다.");

            Console.WriteLine();
            Console.WriteLine("아무 키나 눌러주세요...");
            Console.ReadKey(true);
        }
    }
}
