using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;

namespace Turn_Based_Game
{
    /* 
     Unit class:
        Sets stat variables for all units
    */
    public class Unit
    {
        public required string UnitName { get; set; }

        public required int MaxHp { get; set; }
        public required  int UnitHp {  get; set; }
        
        public required int MaxMp { get; set; }
        public required int UnitMp { get; set; }
        
        public required int UnitAtk { get; set; }
        public required int UnitMag { get; set; }
        
        public int lvl = 0;

        public static void LevelUp()
        {
            // TODO: add code to increase player stats and world lvl
        }
    }

    public class Enemy : Unit
    {
        public int[] enemySkills;
    }

    public class Boss : Enemy
    {
        public int[] bossSkills;
    }
    
    class Program
    {
        // Enums that determines if the player has won or lost a battle. This is to determine if the player needs their stats reset or not
        enum GameState
        {
            GAMEOVER,
            VICTORY
        }

        // Public variables that will be used throughout the battle
        public static string playerName;
        public static string title = "TextVenture!";
        public static string battleText = "Battle!";
        public static string bossText = "Boss Batle!";
        static GameState gameState;
        public static int battleCount = 0;

        // Public player unit that saves all player stat changes
        public static Unit player = new Unit() { UnitName = playerName, MaxHp = 30, UnitHp = 30, MaxMp = 10, UnitMp = 10, UnitAtk = 5, UnitMag = 8 };

        static void Yeild()
        {
            Console.ReadLine();
        }

        static Enemy RandomEnemy()
        {
            Random rnd = new Random();

            Enemy slime = new Enemy() { UnitName = "Slime", MaxHp = 20, UnitHp = 20, MaxMp = 5, UnitMp = 5, UnitAtk = 2, UnitMag = 1 };
            Enemy goblin = new Enemy() { UnitName = "Goblin", MaxHp = 40, UnitHp = 40, MaxMp = 12, UnitMp = 12, UnitAtk = 6, UnitMag = 3 };
            Enemy zombie = new Enemy() { UnitName = "Zombie", MaxHp = 30, UnitHp = 30, MaxMp = 10, UnitMp = 10, UnitAtk = 5, UnitMag = 5 };

            Enemy[] enemies = [ slime, goblin, zombie ];

            Enemy enemy = enemies[rnd.Next(enemies.Length)];

            return enemy;
        }

        static Boss RandomBoss()
        {
            Random rnd = new Random();

            Boss kingSlime = new Boss() { UnitName = "King Slime", MaxHp = 50, UnitHp = 50, MaxMp = 5, UnitMp = 5, UnitAtk = 2, UnitMag = 1 };
            Boss kingGoblin = new Boss() { UnitName = "King Goblin", MaxHp = 50, UnitHp = 50, MaxMp = 12, UnitMp = 12, UnitAtk = 6, UnitMag = 3 };
            Boss kingZombie = new Boss() { UnitName = "King Zombie", MaxHp = 50, UnitHp = 50, MaxMp = 10, UnitMp = 10, UnitAtk = 5, UnitMag = 5 };

            Boss[] bosses = [kingSlime, kingGoblin, kingZombie];

            Boss boss = bosses[rnd.Next(bosses.Length)];

            return boss;
        }

        // The battle method where all battles will take place
        static void Battle()
        {
            Random rnd = new Random();

            // Generates a random enemy
            Enemy enemy = RandomEnemy();

            int[] enemySkills = enemy.enemySkills;
            enemySkills = [0, 1];

            Console.WriteLine("");
            Console.WriteLine("---------------------------");
            Console.WriteLine("      " + battleText + "       ");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
            
            Console.WriteLine(enemy.UnitName + " approaches you!");

            while (player.UnitHp > 0 && enemy.UnitHp > 0) 
            {
                Console.WriteLine("  \n" + playerName);
                Console.WriteLine("HP: " + player.UnitHp + " " + "MP: " + player.UnitMp);
                Console.WriteLine("");
                Console.WriteLine(enemy.UnitName + " HP: " + enemy.UnitHp);
                Console.WriteLine("");
                Console.WriteLine("Your Turn! What will you do?");
                Console.WriteLine("Press [A] to attack!\nPress [H] to heal!(Cost 3 MP)\nPress [F] to cast a magic spell!(Cost 5 MP)");
                
                String playerChoice = Console.ReadLine();

                String newPlayerChoice = playerChoice.ToUpper();

                switch (newPlayerChoice)
                {
                    case "A":
                        enemy.UnitHp -= player.UnitAtk;
                        Console.WriteLine(playerName + " deals " + player.UnitAtk + " damage to the " + enemy.UnitName + "!");
                        Yeild();
                        break;
                    
                    case "H":
                        if (player.UnitMp < 3)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }
                        
                        player.UnitHp += 5;
                        player.UnitMp -= 3;
                        
                        if (player.UnitHp > player.MaxHp)
                        {
                            player.UnitHp = player.MaxHp;
                        }

                        Console.WriteLine(playerName + " recovers 5 HP!");
                        Yeild();
                        break;

                    case "F":
                        if (player.UnitMp < 5)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitMp -= 5;
                        enemy.UnitHp -= player.UnitMag;
                        Console.WriteLine(playerName + " launched a magic attack that deals " + player.UnitMag + " damage to " + enemy.UnitName +"!");
                        Yeild();
                        break;

                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        Yeild();
                        continue;
                }

                Console.WriteLine("Enemy turn!");

                int enemyTurn = rnd.Next(enemySkills.Length);

                switch (enemyTurn)
                {
                    case 0:
                        player.UnitHp -= enemy.UnitAtk;
                        Console.WriteLine(enemy.UnitName + " punches you and deals " + enemy.UnitAtk + " damage!");
                        Yeild();
                        break;

                    case 1:
                        player.UnitHp -= enemy.UnitMag;
                        Console.WriteLine(enemy.UnitName + " enemy launches a magic attack that deals " + enemy.UnitMag + " damage!");
                        Yeild();
                        break;
                }
                

                if (enemy.UnitHp <= 0) 
                {
                    Console.WriteLine("You won the battle!");
                    Unit.LevelUp();
                    battleCount++;
                    gameState = GameState.VICTORY;
                    Yeild();
                    break;
                } 
                else if (player.UnitHp <= 0)
                {
                    Console.WriteLine("Game Over!");
                    gameState = GameState.GAMEOVER;
                    Yeild();
                    break;
                }
            }

            PlayAgain();
        }

        static void BossBattle()
        {
            Random rnd = new Random();

            // Generates a random enemy
            Boss boss = RandomBoss();

            int[] bossSkills = boss.enemySkills;
            bossSkills = [0, 1];

            Console.WriteLine("");
            Console.WriteLine("---------------------------");
            Console.WriteLine("      " + bossText + "       ");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");

            Console.WriteLine(boss.UnitName + " approaches you!");

            while (player.UnitHp > 0 && boss.UnitHp > 0)
            {
                Console.WriteLine("  \n" + playerName);
                Console.WriteLine("HP: " + player.UnitHp + " " + "MP: " + player.UnitMp);
                Console.WriteLine("");
                Console.WriteLine(boss.UnitName + " HP: " + boss.UnitHp);
                Console.WriteLine("");
                Console.WriteLine("Your Turn! What will you do?");
                Console.WriteLine("Press [A] to attack!\nPress [H] to heal!(Cost 3 MP)\nPress [F] to cast a magic spell!(Cost 5 MP)");

                String playerChoice = Console.ReadLine();

                String newPlayerChoice = playerChoice.ToUpper();

                switch (newPlayerChoice)
                {
                    case "A":
                        boss.UnitHp -= player.UnitAtk;
                        Console.WriteLine(playerName + " deals " + player.UnitAtk + " damage to the " + boss.UnitName + "!");
                        Yeild();
                        break;

                    case "H":
                        if (player.UnitMp < 3)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitHp += 5;
                        player.UnitMp -= 3;

                        if (player.UnitHp > player.MaxHp)
                        {
                            player.UnitHp = player.MaxHp;
                        }

                        Console.WriteLine(playerName + " recovers 5 HP!");
                        Yeild();
                        break;

                    case "F":
                        if (player.UnitMp < 5)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitMp -= 5;
                        boss.UnitHp -= player.UnitMag;
                        Console.WriteLine(playerName + " launched a magic attack that deals " + player.UnitMag + " damage to " + boss.UnitName + "!");
                        Yeild();
                        break;

                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        Yeild();
                        continue;
                }

                Console.WriteLine("Enemy turn!");

                int bossTurn = rnd.Next(bossSkills.Length);

                switch (bossTurn)
                {
                    case 0:
                        player.UnitHp -= boss.UnitAtk;
                        Console.WriteLine(boss.UnitName + " punches you and deals " + boss.UnitAtk + " damage!");
                        Yeild();
                        break;

                    case 1:
                        player.UnitHp -= boss.UnitMag;
                        Console.WriteLine(boss.UnitName + " enemy launches a magic attack that deals " + boss.UnitMag + " damage!");
                        Yeild();
                        break;
                }


                if (boss.UnitHp <= 0)
                {
                    Console.WriteLine("You won the battle!");
                    battleCount++;
                    gameState = GameState.VICTORY;
                    Yeild();
                    break;
                }
                else if (player.UnitHp <= 0)
                {
                    Console.WriteLine("Game Over!");
                    gameState = GameState.GAMEOVER;
                    Yeild();
                    break;
                }
            }

            PlayAgain();
        }

        static void PlayAgain()
        {
            Console.WriteLine("Want to play again?\n[Y] Yes\n[N] No");
            
            String playerChoice = Console.ReadLine();

            String newPlayerChoice = playerChoice.ToUpper();

            switch (newPlayerChoice)
            {
                case "Y":
                    if (gameState == GameState.GAMEOVER)
                    {
                        player.UnitHp = player.MaxHp;
                        player.UnitMp = player.MaxMp;
                        battleCount = 0;
                    }

                    if (battleCount == 5)
                    {
                        BossBattle();
                    } else
                    {
                        Battle();
                    }
                    
                    break;

                case "N":
                    Console.WriteLine("Thanks for playing!");
                    Yeild();
                    break;

                default:
                    Console.WriteLine("Thanks for playing!");
                    Yeild();
                    break;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("      " + title + "       ");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
            Console.WriteLine("Welcome! What is your name?");
            playerName = Console.ReadLine();
            Console.WriteLine("Hello " + playerName + "!");

            Battle();
        }
    }
}
