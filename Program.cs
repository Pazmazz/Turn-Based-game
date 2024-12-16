using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using static System.Net.Mime.MediaTypeNames;

namespace Turn_Based_Game
{
    /* 
     *Unit class:
     *   Sets stat variables for all units
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
        
        public int lvl = 1;

        public void LevelUp()
        {
            lvl++;

            MaxHp += 5;
            UnitHp = MaxHp;
            MaxMp += 4;
            UnitMp = MaxMp;
            UnitAtk += 2;
            UnitMag += 2;
        }
    }

    /* 
     *Enemy class:
     *   Inherets the unit class but with a special LevelUp() function
     *
     *   Now LevelUp() multiplies all stats by the world level to ensure the enemy stats scale with the player level
    */
    public class Enemy : Unit
    {
        public int[] enemySkills;

        public void LevelUp(int worldLvl)
        {
            lvl = worldLvl;

            MaxHp += 5 * worldLvl;
            UnitHp = MaxHp;
            MaxMp += 4 * worldLvl;
            UnitMp = MaxMp;
            UnitAtk += 2 * worldLvl;
            UnitMag += 2 * worldLvl;
        }
    }

    /* 
     *Boss class:
     *  Inherents the enemy class. Not much difference between the two but helps differentiate between a boss monster and a normal enemy
    */
    public class Boss : Enemy
    {
        public int[] bossSkills;
    }
    
    class Program
    {
        /* 
         *Enums that determines if the player has won or lost a battle. This is to determine if the player needs their stats reset or not
        */
        enum GameState
        {
            GAMEOVER,
            VICTORY
        }

        /* 
         * Public variables that will be used throughout the battle
        */
        public static string playerName;
        public static string title = "TextVenture!";
        public static string battleText = "Battle!";
        public static string bossText = "Boss Batle!";
        static GameState gameState;
        public static int battleCount = 0;
        public static int worldlvl = 1;

        /* 
         * Public player unit that saves all player stat changes
        */
        public static Unit player = new Unit() { UnitName = playerName, MaxHp = 30, UnitHp = 30, MaxMp = 10, UnitMp = 10, UnitAtk = 5, UnitMag = 8 };

        /*
         * The Yeild() function exist to pause the text so the player has time to read everything going on in the battle
        */
        static void Yeild()
        {
            Console.ReadLine();
        }

        /*
         * This function spawns in three new enemy objects, puts them into an enemies[] array, and then a random enemy is selected from that array
         * 
         * The selected enemy is then leveled up
        */
        static Enemy RandomEnemy()
        {
            Random rnd = new Random();

            Enemy slime = new Enemy() { UnitName = "Slime", MaxHp = 13, UnitHp = 13, MaxMp = 20, UnitMp = 20, UnitAtk = 2, UnitMag = 1 };
            Enemy goblin = new Enemy() { UnitName = "Goblin", MaxHp = 20, UnitHp = 20, MaxMp = 20, UnitMp = 20, UnitAtk = 5, UnitMag = 3 };
            Enemy zombie = new Enemy() { UnitName = "Zombie", MaxHp = 15, UnitHp = 15, MaxMp = 20, UnitMp = 20, UnitAtk = 4, UnitMag = 5 };

            Enemy[] enemies = [ slime, goblin, zombie ];

            Enemy enemy = enemies[rnd.Next(enemies.Length)];

            enemy.LevelUp(worldlvl);

            return enemy;
        }

        /*
         * This function spawns in three new boss objects, puts them into an bosses[] array, and then a random boss is selected from that array
        */
        static Boss RandomBoss()
        {
            Random rnd = new Random();

            Boss kingSlime = new Boss() { UnitName = "The Slime King", MaxHp = 70, UnitHp = 70, MaxMp = 70, UnitMp = 70, UnitAtk = 15, UnitMag = 18 };
            Boss kingGoblin = new Boss() { UnitName = "The Goblin King", MaxHp = 70, UnitHp = 70, MaxMp = 70, UnitMp = 70, UnitAtk = 15, UnitMag = 18 };
            Boss kingZombie = new Boss() { UnitName = "The Zombie King", MaxHp = 70, UnitHp = 70, MaxMp = 70, UnitMp = 70, UnitAtk = 15, UnitMag = 18 };

            Boss[] bosses = [kingSlime, kingGoblin, kingZombie];

            Boss boss = bosses[rnd.Next(bosses.Length)];

            return boss;
        }

        /*
         * The battle method where all battles will take place
        */
        static void Battle()
        {
            Random rnd = new Random();

            // Generates a random enemy
            Enemy enemy = RandomEnemy();

            /*
             * Array if two numbers which determine which attack the enemy will use during their turn
            */
            enemy.enemySkills = [0, 1];

            /* 
             * Variables that determine the mp cost for each skill
             * It's added to the player level so it scales properly throughout the game
             * Heal amount scales of the players max hp and heals 40% percent of it
            */
            int healCost = 3 + worldlvl;
            int magCost = 5 + worldlvl;
            double healAmount = (int)player.MaxHp * 0.4;

        Console.WriteLine("");
            Console.WriteLine("---------------------------");
            Console.WriteLine("      " + battleText + "       ");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
            
            Console.WriteLine(enemy.UnitName + " approaches you!");

            /*
             * A while loop that takes user input during the player turn
            */
            while (player.UnitHp > 0 && enemy.UnitHp > 0) 
            {
                Console.WriteLine("  \n" + playerName + "   Level " + player.lvl);
                Console.WriteLine("HP: " + player.UnitHp + " " + "MP: " + player.UnitMp);
                Console.WriteLine("");
                Console.WriteLine(enemy.UnitName + "    Level " + enemy.lvl + "\nHP: " + enemy.UnitHp);
                Console.WriteLine("");
                Console.WriteLine("Your Turn! What will you do?");
                Console.WriteLine("Press [A] to attack!\nPress [H] to heal 40% of your maximum HP!(Cost " + healCost + " MP)\nPress [F] to cast a magic spell!(Cost " + magCost + " MP)");
                
                
                /*
                 * Takes user input and converts the input to a capital letter if it already isn't
                */
                String playerChoice = Console.ReadLine();

                String newPlayerChoice = playerChoice.ToUpper();
                
                /*
                 * This statement checks if the enemy is dead, if so, level up the player, 
                 * increase the battle count and world level, and set the game state to VICTORY:
                 * if (enemy.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            player.LevelUp();
                            battleCount++;
                            worldlvl++;
                            gameState = GameState.VICTORY;
                            Yeild();
                            PlayAgain();
                        }
                */
                switch (newPlayerChoice)
                {
                    /*
                     * Player uses a regular attack dealing damage equivalent to the player's attack(player.UnitAtk)
                    */
                    case "A":
                        enemy.UnitHp -= player.UnitAtk;
                        Console.WriteLine(playerName + " deals " + player.UnitAtk + " damage to the " + enemy.UnitName + "!");
                        Yeild();
                        
                        if (enemy.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            player.LevelUp();
                            battleCount++;
                            worldlvl++;
                            gameState = GameState.VICTORY;
                            Yeild();
                            PlayAgain();
                        }
                        break;
                    
                    /*
                     * Player heals some HP if they have enough MP
                    */
                    case "H":
                        if (player.UnitMp < 3)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }
                        
                        player.UnitHp += (int)healAmount;
                        player.UnitMp -= healCost;
                        
                        if (player.UnitHp > player.MaxHp)
                        {
                            player.UnitHp = player.MaxHp;
                        }

                        Console.WriteLine(playerName + " recovers " + healAmount + " HP!");
                        Yeild();
                        break;
                    
                    /*
                     * Player cast a magic spell if they have enough HP. The damage dealt is equivalent to the player's magic(player.UnitMag)
                    */
                    case "F":
                        if (player.UnitMp < 5)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitMp -= magCost;
                        enemy.UnitHp -= player.UnitMag;
                        Console.WriteLine(playerName + " launched a magic attack that deals " + player.UnitMag + " damage to " + enemy.UnitName +"!");
                        Yeild();
                        
                        if (enemy.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            player.LevelUp();
                            battleCount++;
                            worldlvl++;
                            gameState = GameState.VICTORY;
                            Yeild();
                            PlayAgain();
                        }

                        break;

                    /*
                     * In the user doesn't input any of the three options from above, throw an error and allow them to try again
                    */
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        Yeild();
                        continue;
                }

                Console.WriteLine("Enemy turn!");

                int enemyTurn = rnd.Next(enemy.enemySkills.Length);


                /*
                 * This statement checks if the player is dead, if so, set the game state to GAMEOVER and return to the PlayAgain() screen:
                 * if (player.UnitHp <= 0)
                        {
                            Console.WriteLine("Game Over!");
                            gameState = GameState.GAMEOVER;
                            Yeild();
                            PlayAgain();
                        }
                */
                switch (enemyTurn)
                {
                    /*
                     * Enemy uses a regular attack dealing damage equivalent to the enemy's attack(enemy.UnitAtk)
                    */
                    case 0:
                        player.UnitHp -= enemy.UnitAtk;
                        Console.WriteLine(enemy.UnitName + " punches you and deals " + enemy.UnitAtk + " damage!");
                        Yeild();

                        if (player.UnitHp <= 0)
                        {
                            Console.WriteLine("Game Over!");
                            gameState = GameState.GAMEOVER;
                            Yeild();
                            PlayAgain();
                        }
                        
                        break;

                    /*
                     * Enemy cast a magic spell if they have enough HP. The damage dealt is equivalent to the enemy's magic(enemy.UnitMag)
                    */
                    case 1:
                        enemy.UnitMp -= 2;
                        if (enemy.UnitMp <= 0)
                        {
                            Console.WriteLine(enemy.UnitName + " tried a magic but failed!");
                            Yeild();
                            break;
                        }
                        player.UnitHp -= enemy.UnitMag;
                        Console.WriteLine(enemy.UnitName + " enemy launches a magic attack that deals " + enemy.UnitMag + " damage!");
                        Yeild();

                        if (player.UnitHp <= 0)
                        {
                            Console.WriteLine("Game Over!");
                            gameState = GameState.GAMEOVER;
                            Yeild();
                            PlayAgain();
                        }
                        break;
                }
            }

            PlayAgain();
        }

        static void BossBattle()
        {
            Random rnd = new Random();

            /* 
            * Generates a random boss
            */
            Boss boss = RandomBoss();

            /*
             * Array if two numbers which determine which attack the boss will use during their turn
            */
            int[] bossSkills = boss.enemySkills;
            bossSkills = [0, 1];

            /* 
             * Variables that determine the mp cost for each skill
             * It's added to the player level so it scales properly throughout the game
             * Heal amount scales of the players max hp and heals 40% percent of it
            */
            int healCost = 3 + worldlvl;
            int magCost = 5 + worldlvl;
            double healAmount = (int)player.MaxHp * 0.4;

            Console.WriteLine("");
            Console.WriteLine("---------------------------");
            Console.WriteLine("      " + bossText + "       ");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");

            Console.WriteLine(boss.UnitName + " approaches you!");

            /*
             * A while loop that takes user input during the player turn
            */
            while (player.UnitHp > 0 && boss.UnitHp > 0)
            {
                Console.WriteLine("  \n" + playerName + "   Level " + player.lvl);
                Console.WriteLine("HP: " + player.UnitHp + " " + "MP: " + player.UnitMp);
                Console.WriteLine("");
                Console.WriteLine(boss.UnitName + "    Level " + boss.lvl + "\nHP: " + boss.UnitHp);
                Console.WriteLine("");
                Console.WriteLine("Your Turn! What will you do?");
                Console.WriteLine("Press [A] to attack!\nPress [H] to heal 40% of your maximum HP!(Cost " + healCost + " MP)\nPress [F] to cast a magic spell!(Cost " + magCost + " MP)");

                String playerChoice = Console.ReadLine();

                String newPlayerChoice = playerChoice.ToUpper();

                switch (newPlayerChoice)
                {
                    /*
                     * Player uses a regular attack dealing damage equivalent to the player's attack(player.UnitAtk)
                    */
                    case "A":
                        boss.UnitHp -= player.UnitAtk;
                        Console.WriteLine(playerName + " deals " + player.UnitAtk + " damage to the " + boss.UnitName + "!");
                        Yeild();

                        if (boss.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            player.LevelUp();
                            battleCount++;
                            gameState = GameState.VICTORY;
                            Yeild();
                            PlayAgain();
                        }
                        break;

                    /*
                     * Player heals some HP if they have enough MP
                    */
                    case "H":
                        if (player.UnitMp < 3)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitHp += (int)healAmount;
                        player.UnitMp -= healCost;

                        if (player.UnitHp > player.MaxHp)
                        {
                            player.UnitHp = player.MaxHp;
                        }

                        Console.WriteLine(playerName + " recovers " + healAmount + " HP!");
                        Yeild();
                        break;

                    /*
                     * Player cast a magic spell if they have enough HP. The damage dealt is equivalent to the player's magic(player.UnitMag)
                    */
                    case "F":
                        if (player.UnitMp < 5)
                        {
                            Console.WriteLine("Not enought MP!");
                            break;
                        }

                        player.UnitMp -= magCost;
                        boss.UnitHp -= player.UnitMag;
                        Console.WriteLine(playerName + " launched a magic attack that deals " + player.UnitMag + " damage to " + boss.UnitName + "!");
                        Yeild();

                        if (boss.UnitHp <= 0)
                        {
                            Console.WriteLine("You won the battle!");
                            player.LevelUp();
                            battleCount++;
                            gameState = GameState.VICTORY;
                            Yeild();
                            PlayAgain();
                        }
                        break;

                    /*
                     * In the user doesn't input any of the three options from above, throw an error and allow them to try again
                    */
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        Yeild();
                        continue;
                }

                Console.WriteLine("Boss turn!");

                int bossTurn = rnd.Next(bossSkills.Length);

                switch (bossTurn)
                {
                    /*
                     * Boss uses a regular attack dealing damage equivalent to the boss's attack(boss.UnitAtk)
                    */
                    case 0:
                        player.UnitHp -= boss.UnitAtk;
                        Console.WriteLine(boss.UnitName + " punches you and deals " + boss.UnitAtk + " damage!");
                        Yeild();

                        if (player.UnitHp <= 0)
                        {
                            Console.WriteLine("Game Over!");
                            gameState = GameState.GAMEOVER;
                            Yeild();
                            PlayAgain();
                        }
                        break;

                    /*
                     * Enemy cast a magic spell if they have enough HP. The damage dealt is equivalent to the enemy's magic(enemy.UnitMag)
                    */
                    case 1:
                        boss.UnitMp -= 2;
                        player.UnitHp -= boss.UnitMag;
                        if (boss.UnitMp <= 0)
                        {
                            Console.WriteLine(boss.UnitName + " tried a magic but failed!");
                            Yeild();
                            break;
                        }
                        Console.WriteLine(boss.UnitName + " enemy launches a magic attack that deals " + boss.UnitMag + " damage!");
                        Yeild();

                        if (player.UnitHp <= 0)
                        {
                            Console.WriteLine("Game Over!");
                            gameState = GameState.GAMEOVER;
                            Yeild();
                            PlayAgain();
                        }
                        break;
                }
            }

            PlayAgain();
        }

        static void PlayAgain()
        {
            if (battleCount > 5)
            {
                Console.WriteLine("Congradulations, you won!\nThanks for playing!");
                Yeild();
                Environment.Exit(0);
            }
            
            Console.WriteLine("Want to play again?\n[Y] Yes\n[N] No");
            
            String playerChoice = Console.ReadLine();

            String newPlayerChoice = playerChoice.ToUpper();

            switch (newPlayerChoice)
            {
                case "Y":
                    if (gameState == GameState.GAMEOVER)
                    {
                        player.lvl = 1;
                        player.UnitHp = 30;
                        player.UnitMp = 10;
                        player.UnitAtk = 5;
                        player.UnitMag = 8;
                        battleCount = 0;
                        worldlvl = 1;
                        Battle();
                    }

                    if (battleCount == 5)
                    {
                        BossBattle();
                    } else
                    {
                        Battle();
                    }
                    
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
            Console.WriteLine("Welcome to the battle gauntlet!\nIn order to win, you must win 5 battles and defeat the boss at the very end!\nFirst let's get your name. Enter you name here:");
            playerName = Console.ReadLine();
            Console.WriteLine("Hello " + playerName + "!\nYou ready for your first battle?\nLet's go!");

            Battle();
        }
    }
}
