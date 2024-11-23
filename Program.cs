using System;
using System.Reflection.Metadata.Ecma335;

namespace Turn_Based_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            
            int playerHP = 40;
            int enemyHP = 30;

            int playerMP = 20;

            int playerAttack = 5;
            int playerMagic = 8;
            
            int enemyAttack = 5;
            int enemyMagic = 8;

            int [] enemySkills = {0, 1};
            
            Console.WriteLine("Welcome! What is your name?");
            String playerName = Console.ReadLine();
            Console.WriteLine("Hello " + playerName + "!");
            Console.WriteLine("An enemy is approaching you! Get ready!");

            while (playerHP > 0 && enemyHP > 0) 
            {
                Console.WriteLine("  \n" + playerName);
                Console.WriteLine("HP: " + playerHP + " " + "MP: " + playerMP);
                Console.WriteLine("Enemy HP: " + enemyHP);
                Console.WriteLine("Your Turn! What will you do?");
                Console.WriteLine("Press [A] to attack!\nPress [H] to heal!(Cost 5 MP)\nPress [F] to deal fire damage!(Cost 10 MP)");
                
                String playerChoice = Console.ReadLine();

                switch (playerChoice)
                {
                    case "a":
                        enemyHP -= playerAttack;
                        Console.WriteLine(playerName + " deals " + playerAttack + " damage to the enemy!");
                        break;
                    
                    case "h":
                        if (playerMP < 5)
                        {
                            Console.WriteLine("Not enought MP!");
                        }
                        
                        playerHP += 5;
                        playerMP -= 5;
                        Console.WriteLine(playerName + " recovers " + playerAttack + " HP!");
                        break;

                    case "f":
                        if (playerMP < 10)
                        {
                            Console.WriteLine("Not enought MP!");
                        }

                        playerMP -= 10;
                        enemyHP -= playerMagic;
                        Console.WriteLine(playerName + " deals " + playerMagic + " damage to the enemy!");
                        break;
                }

                Console.WriteLine("Enemy turn!");

                int enemyTurn = rnd.Next(enemySkills.Length);

                switch (enemyTurn)
                {
                    case 0:
                        playerHP -= enemyAttack;
                        Console.WriteLine("The enemy punches you and deals " + enemyAttack + " damage!");
                        break;

                    case 1:
                        playerHP -= enemyMagic;
                        Console.WriteLine("The enemy launches an ice attack that deals " + enemyMagic + " damage!");
                        break;
                }
                

                if (enemyHP <= 0) 
                {
                    Console.WriteLine("You won the battle!");
                    break;
                } 
                else if (playerHP <= 0)
                {
                    Console.WriteLine("Game Over!");
                    break;
                }
            }

            Console.WriteLine("Thanks for playing!");
        }
    }
}
