/*Program: Game of Nim
 * Programmers: JOhn Shannon, Jason Holdridge, Charles Penzien
 * Assignment: Seminar 6 Group Project
 * Description: Plays a game of Nim with the user, where the computer attempts to always find the optimal play
 * Concepts Practiced: Doubly Linked List, XOR functionality, Input Validation, Polymorphism,
 * ASCII art, loop control, Exception handling, NameSpace separation
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfNim
{
    class NimGame //driver class
    {

        static void Main(string[] args)
        {
            #region Variables

            HumanPlayer player1;
            Player player2;
            Player turnPlayer;
            Player other;
            bool playAgain;
            bool humanFirst;
            int pileNumber;

            #endregion


            try
            {
                Introduction();
                HowToPlay();

                //Initial game loop, starts new games
                Console.Clear();
                Console.WriteLine("Please enter your name:");
                player1 = new HumanPlayer(Console.In.ReadLine());
                player2 = new ComputerPlayer();

                do
                {
                    //Sets up new List pile and Player turn assignment
                    #region Set-up

                    List<NimPile> Piles = new List<NimPile>(); //starts a new pile for the next game

                    //Allows the user to determine which player goes first, themselves or the computer
                    Console.Out.WriteLine("Hello, " + player1.Name + ", would you like to go first (yes or no)?");
                    humanFirst = HumanFirst(Console.In.ReadLine());
                    if (humanFirst)
                        Console.Out.WriteLine(player1.Name + " wants to go first.");
                    else
                        Console.Out.WriteLine("Computer will go first.");

                    //TODO: new Player Types - Jason - players established above

                    //Ask user how many piles to play with and validate input
                    Console.Out.WriteLine(player1.Name + ", how many piles of sticks would you like to play with?");
                    pileNumber = NumberValidate(Console.In.ReadLine());

                    //TODO:Implement Pile selection 
                    for (int i = 0; i < pileNumber; i++)
                    {
                        //ToDO: implement number of stick selection and input validation
                        Console.Out.WriteLine(player1.Name + ", please enter the number of sticks for pile #" + (i + 1).ToString() + ":");

                        //ToDO: Implement pile creation for how many piles are needed with how many sticks are in the pile
                        NimPile temp = new NimPile(NumberValidate(Console.In.ReadLine()));
                        Piles.Add(temp);
                    };

                    //TODO: Implement interface for pile selection
                    PileDisplay(Piles);

                    //TODO: Make turnPlayer point to the reference of the first turn decided by human player
                    if (humanFirst)
                    {
                        turnPlayer = player1;
                        other = player2;
                    }
                    else
                    {
                        turnPlayer = player2;
                        other = player1;
                    }

                    #endregion
                    

                    //Handles the turns of the player. This involves treating the turnPlayer polymorphicly, as the actual turns are handled in the ComputerPlayer
                    //and HumanPlayer classes. Then removes any empty piles from the list and swaps the players if the game is not over.
                    #region Turns

                    do
                    {



                        turnPlayer.TakeTurn(Piles);
                        Console.WriteLine("Press Enter to Continue");
                        Console.ReadLine();

                        //removes empty piles from the list, keeping the presentation simple
                        RemoveEmptyPiles(Piles);

                        PileDisplay(Piles);

                        //if statement for loop control, bypasses the turnPlayer swapping when all piles are empty
                        if (GameOver(Piles))
                        {
                            break;
                        }

                        //swaps the current player and the other player
                        if (turnPlayer.IsA(player1))
                        {
                            turnPlayer = player2;
                            other = player1;
                        }
                        else
                        {
                            turnPlayer = player1;
                            other = player2;
                        }

                    } while (true);//creates an infinite loop that will be controlled by an if statement instead

                    #endregion


                    //TODO: Award current player with a win And prompt for replay
                    #region Post-Game

                    Console.WriteLine(turnPlayer.ToString() + " Wins");
                    turnPlayer.Win(other);

                    Console.Out.WriteLine("The computer has " + player2.numOfWins + " win(s) and you have " + player1.numOfWins + " win(s).");

                    Console.Out.WriteLine("Hello, " + player1.Name + ", would you like to play again (yes or no)?");
                    playAgain = HumanFirst(Console.In.ReadLine());


                    #endregion

                
                
                } while (playAgain); //TODO:Implement Choice and input validation 


                Console.Clear();
                Console.WriteLine("Thank You For Playing " + player1.ToString() + "!");

            }
            catch (Exception ex)//generic catch for testing purposes
            {
                Console.WriteLine(ex.Message + "\n\n\n" + ex.StackTrace);
            }

            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }



        //Static text to introduce the player to the game, incase it is their first time
        //Also helped to outline what needed to happen
        #region Static Text

        private static void Introduction()
        {
            Console.WriteLine("Welcome to : ");



            Console.WriteLine(".__   __.  __  .___  ___.");
            Console.WriteLine("|  \\ |  | |  | |   \\/   |");
            Console.WriteLine("|   \\|  | |  | |  \\  /  |");
            Console.WriteLine("|  . `  | |  | |  |\\/|  |");
            Console.WriteLine("|  |\\   | |  | |  |  |  |");
            Console.WriteLine("|__| \\__| |__| |__|  |__|");

            Console.WriteLine("The goal of the game is to be the one who\npulls the last stick from the last pile.");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }
        private static void HowToPlay()
        {
            Console.Clear();
            Console.WriteLine("How To Play:");
            Console.WriteLine("\n1. There are two players, one human and one computer.\n");
            Console.WriteLine("2. Human player decides what order the players take turns.\n");
            Console.WriteLine("3. The human player will decide how many piles\n\tthere are and how many sticks are in each pile\n");
            Console.WriteLine("4. Players will take turns, pulling however many \n\tticks out of one pile at a time.\n");
            Console.WriteLine("5. This continues until the last stick(s) is(are)\n\tremoved from the last nonempty pile.\n");
            Console.WriteLine("6. When no more sticks are within a pile, the player\n\twho removed the last stick is declared the winner.\n");
            Console.WriteLine("7. The human player then may choose to play again.\n");
            Console.WriteLine("\n\nPress Enter to Continue");
            Console.ReadLine();
        }

        #endregion


        #region Input Validations

        private static bool HumanFirst(string input)
        {
            //Calls validate to determine if YES or NO entered
            string answer = Validate(input);
            //Returns boolean value depending on user preference
            if (answer.ToUpper() == "YES")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string Validate(string input)
        {
            //Loops through until user input is YES or NO
            bool valid = false;
            do
            {
                if (input.ToUpper() != "YES" && input.ToUpper() != "NO")
                {
                    Console.Out.WriteLine("You must enter \"yes\" or \"no\":");
                    input = Console.In.ReadLine();
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            } while (valid);
            return input;
        }
        private static int NumberValidate(string input)
        {
            //Loops though until the user inputs an integer that is greater than zero (0)
            //This works for both number of piles and number of sticks
            int number;
            bool valid = false;
            do
            {
                if (int.TryParse(input, out number) && number > 0)
                {
                    valid = false;
                }
                else
                {
                    Console.Out.WriteLine("You must enter an integer greater than zero for the number of piles or number of sticks:");
                    input = Console.In.ReadLine();
                    valid = true;
                }
            } while (valid);
            return number;
        }

        public static int NumberValidate(string input, List<NimPile> list)
        {
            //Loops though until the user inputs an integer that is greater than zero (0) and within the number of pile range
            //This works for the selection of a NimPile

            //Note from John: made this a public static so that the main body of the program and humanPlayer class
            //can both easily use for input validation.

            int number;
            bool valid = false;
            do
            {
                if (int.TryParse(input, out number) && (number > 0 && number <= list.Count))
                {
                    valid = false;
                }
                else
                {
                    Console.Out.WriteLine("You must enter a valid pile number:");
                    input = Console.In.ReadLine();
                    valid = true;
                }
            } while (valid);
            return number;
        }

        public static int NumberValidate(string input, NimPile Pile)
        {
            //Loops though until the user inputs an integer that is greater than zero (0) and within the number of pile range
            //This works for the selection of a NimPile

            //Same note as above

            int number;
            bool valid = false;
            do
            {
                if (int.TryParse(input, out number) && (number > 0 && number <= Pile.PileSize()))
                {
                    valid = false;
                }
                else
                {
                    Console.Out.WriteLine("You must enter a valid number of sticks:");
                    input = Console.In.ReadLine();
                    valid = true;
                }
            } while (valid);
            return number;
        }


        #endregion
        

        //Displays the number of piles and the number of sticks in each pile
        private static void PileDisplay(List<NimPile> list)  //Changed from PileSelection to better describe purpose
        {
            int index = 1;
            //Note from John: interface for actually choosing a pile moved to humanPlayer.TakeTurn(list<NimPile>)

            Console.Clear();
            foreach (NimPile pile in list)
            {
                Console.Out.WriteLine("Pile #" + index.ToString() + " has " + pile.ToString() + " sticks.");
                index++;
            }
        }

        //checks if the game has ended. used to check each pile individualy, but when 
        //piles started getting removed on emptying, winning parameters changed
        private static bool GameOver(List<NimPile > Piles) 
        {
            if (Piles.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Removes Empty Piles, makes keeping track of everything easier
        private static void RemoveEmptyPiles(List<NimPile> Piles)
        {
            if (Piles.Count == 0) //exits on the event of an empty list being passed
            {
                return;
            }
            for (int i = 0; i < Piles.Count; i++)
            {
                if (Piles[i].PileSize() == 0)
                {
                    Piles.RemoveAt(i);
                }
            }
        }
    }

   

}