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
    }

    public class Enemy : Unit
    {
        public required int EnemyId;

        public int[] enemySkills;
    }
    
    class Program
    {
        enum GameState
        {
            GAMEOVER,
            VICTORY
        }

        // Public text variablesthat will be used throughout the battle
        public static string playerName;
        public static string title = "TextVenture!";
        public static string battleText = "Battle!";
        static GameState gameState;

        // Public player unit that saves all player state changes
        public static Unit player = new Unit() { UnitName = playerName, MaxHp = 30, UnitHp = 30, MaxMp = 10, UnitMp = 10, UnitAtk = 5, UnitMag = 8 };

        static void Yeild()
        {
            Console.ReadLine();
        }

        static Enemy RandomEnemy()
        {
            Random rnd = new Random();

            Enemy slime = new Enemy() { EnemyId = 0, UnitName = "Slime", MaxHp = 20, UnitHp = 20, MaxMp = 5, UnitMp = 5, UnitAtk = 2, UnitMag = 1 };
            Enemy goblin = new Enemy() { EnemyId = 1, UnitName = "Goblin", MaxHp = 40, UnitHp = 40, MaxMp = 12, UnitMp = 12, UnitAtk = 6, UnitMag = 3 };
            Enemy zombie = new Enemy() { EnemyId = 2, UnitName = "Zombie", MaxHp = 30, UnitHp = 30, MaxMp = 10, UnitMp = 10, UnitAtk = 5, UnitMag = 5 };

            Enemy[] enemies = [ slime, goblin, zombie ];

            Enemy enemy = enemies[rnd.Next(enemies.Length)];

            return enemy;
        }

        // The battle method where all battles will take place
        static void Battle()
        {
            Random rnd = new Random();

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
                Console.WriteLine("Press [A] to attack!\nPress [H] to heal!(Cost 3 MP)\nPress [F] to deal fire damage!(Cost 5 MP)");
                
                String playerChoice = Console.ReadLine();

                switch (playerChoice)
                {
                    case "a":
                        enemy.UnitHp -= player.UnitAtk;
                        Console.WriteLine(playerName + " deals " + player.UnitAtk + " damage to the " + enemy.UnitName + "!");
                        Yeild();

                        if (enemy.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            break;
                        }

                        break;
                    
                    case "h":
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

                    case "f":
                        if (player.UnitMp < 5)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitMp -= 5;
                        enemy.UnitHp -= player.UnitMag;
                        Console.WriteLine(playerName + " deals " + player.UnitMag + " damage to the " + enemy.UnitName +"!");
                        Yeild();

                        if (enemy.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            break;
                        }

                        break;
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

            switch (playerChoice)
            {
                case "y":
                    if (gameState == GameState.GAMEOVER)
                    {
                        player.UnitHp = player.MaxHp;
                        player.UnitMp = player.MaxMp;
                    }
                    Battle();
                    break;

                case "n":
                    Console.WriteLine("Thanks for playing!");
                    break;

                case default:
                    Console.WriteLine("Thanks for playing!");
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
