/*Program: Game of Nim
 * Programmers: JOhn Shannon, Jason Holdridge, Charles Penzien
 * Assignment: Seminar 6 Group Project
 * Description: Defines the Human player's turn and the Computer's Turn (Logic)
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
    abstract class Player
    {
        //Possible members : score, methods : TakeTurn(), Win()
        private int score;
        public int numOfWins 
        { 
            get { return score; } 
            private set { score = value; } 
        }  //tallies the player's number of wins during the current game execution
        

        public Player()
        {
            numOfWins = 0; //Constructor
        }

        public virtual void Win(Player loser) //virtual to allow some pretty displays
        {
            numOfWins++;
        }

        public abstract void TakeTurn(List<NimPile> Piles);
        public abstract bool IsA(Player P);
    }

    class ComputerPlayer : Player
    {
        //Defines the computer's turn
        public override void TakeTurn(List<NimPile> Piles)
        {

            //going for the victory
            if (Piles.Count == 1) 
            {
                if (Piles[0].RemoveSticks(Piles[0].PileSize()))
                {
                    Console.WriteLine("Computer removes all sticks from the last pile.");
                    return;
                }
            }
            NimPile nimSum = new NimPile(); // computes the nimsum
            nimSum = CalculateNimSum(Piles);


                //Console.WriteLine("Current NIMSUM: " + nimSum.PileSize()); For Testing
            //Lets the computer simulate how to best approach the turn
            Simulate(Piles, nimSum);
        }


        //Calculates the Nimsum for the all piles in the List
        private static NimPile CalculateNimSum(List<NimPile> Piles) //calculates the nimsum
        {
            NimPile Temp = new NimPile(0);
            foreach (NimPile N in Piles)
            {
                Temp = NimPile.NimSum(Temp.First, N.First);
            }

            return Temp;
        }


        //Simulates removing sticks and adding if they do not work
        private static void Simulate(List<NimPile> Piles, NimPile nimSum)
        {
            NimPile N = new NimPile(0);
            

            if (nimSum.PileSize() == 0)
            {
                int tempSticks;
                int ind = FindLargestPile(Piles);


                //If the nimSum starts at 0, either takes an entire pile or all but one of a pile, depending on what would make it more likely to win.
                //If there are an even number of piles left, the computer wants to keep it that way, as removing one each turn would result in the opponent's victory
                //If there is an odd number of piles, an entire pile is removed so that if the human player blindly removes entire piles, computer will be at an advantage
                //These quantities are used to let the game continue to be fast-paced
                if ((Piles.Count % 2) == 0)
                {
                    tempSticks = Piles[ind].PileSize() - 1;
                    if (Piles[ind].RemoveSticks(tempSticks))
                    {
                        Console.WriteLine("The computer has removed " + tempSticks + " stick from the " + (ind + 1).ToString() + " pile");
                        return;
                    }
                    else
                    {
                        Piles[0].RemoveSticks(1);
                        Console.WriteLine("The computer removes the last stick from the first Pile");
                        return;
                    }
                }
                else
                {
                    tempSticks = Piles[ind].PileSize();
                    if (Piles[ind].RemoveSticks(tempSticks))
                    {
                        Console.WriteLine("The computer removes " + tempSticks.ToString() + " sticks from the "+ (ind+1).ToString() +" Pile");
                        return;
                    }
                }
            }


            //goes through each pile, trying to remove the  nimSum of sticks from each pile, if successful reports the move
            for (int i = 0; i < Piles.Count; i++)
            {
                if (nimSum.PileSize() < Piles[i].PileSize())
                {
                    if (Piles[i].RemoveSticks(nimSum.PileSize()))
                    {
                        if (N.PileSize() == (CalculateNimSum(Piles)).PileSize())
                        {
                            //tells the user that the sticks were removed as the computer's move
                            Console.WriteLine("The computer removed " + nimSum.ToString() + " sticks from pile " + (i + 1).ToString()); 
                            return;
                        }
                        else
                        {
                            //If the nimsum of the new pile is not 0, undo the stick removal.
                            Piles[i].AddSticks(nimSum.PileSize());
                        }
                    }
                }
            }


            //If the computer cannot create a 0 nimsum or the nimsum is larger than sny pile, tries to remove elements from the largest pile
            //To figure out how many, it XOR's out the largest pile from the nimsum and removes the difference from the pile
            //Inspired by the 5 pile example and Jason's discussion post
            int index = FindLargestPile(Piles);
            int tempStickS = Piles[index].PileSize();
            NimPile sum = NimPile.NimSum(nimSum.First, Piles[index].First);
            int toRemove = tempStickS - sum.PileSize();

            if (Piles[index].RemoveSticks(toRemove))
                {
                    Console.WriteLine("Computer removed " + toRemove.ToString() + " sticks from pile " + (index + 1).ToString());
                    return;
                }
                else if (Piles[index].RemoveSticks(1))
                {
                    //If all else fails, attempts to remove 1 stick from the largest pile
                    //meant to be a last resort
                    Console.WriteLine("Computer removed 1 sticks from pile " + (index + 1).ToString());
                }
                else
                {
                    throw new Exception("Computer Encountered a Situation it cannot handle");
                }


        }

        public static int FindLargestPile(List<NimPile> N)//searches Piles for the largest Pile
        {
            int index = 0 , largest = 0;

            for (int i = 0; i < N.Count; i++)
            {
                if (N[i].PileSize() > largest)
                {
                    index = i;
                    largest = N[i].PileSize();
                }
            }

            return index;
        }


        public override bool IsA(Player N) //For Player Type Comparison
        {
            return this.GetType() == N.GetType();
        }

        public override string ToString() //For printing, like HumanPlayer name
        {
            return "Computer";
        }
        public override void Win(Player loser) //Taunt the user for not being as good
        {
            Console.WriteLine(  " _______   _______  _______  _______     ___   .___________.\n" +
                                "|       \\ |   ____||   ____||   ____|   /   \\  |           |\n" +
                                "|  .--.  ||  |__   |  |__   |  |__     /  ^  \\ `---|  |----`\n" +
                                "|  |  |  ||   __|  |   __|  |   __|   /  /_\\  \\    |  |     \n" +
                                "|  '--'  ||  |____ |  |     |  |____ /  _____  \\   |  |     \n" +
                                "|_______/ |_______||__|     |_______/__/     \\__\\  |__|   \n" );
            base.Win(loser);

            Console.Out.WriteLine("You LOSE, Sorry!");
            Console.WriteLine("Press Enter to Continue");
            Console.ReadLine();
        }

    }

    class HumanPlayer : Player
    {
        private string name;

        public HumanPlayer(string name)
        {
            this.name = name;
        }

        public string Name          //Property to communicate with human player
        {
            get { return name; }
        }


        //Defines the Human user's name
        public override void TakeTurn(List<NimPile> Piles)
        {
            int selectedPile;
            int selectedSticks;

            //Prompts the user and makes sure they enter a proper pile indicator
            Console.Out.WriteLine("Please enter a pile number from which to select sticks:");
            selectedPile = NimGame.NumberValidate(Console.In.ReadLine(), Piles);

            //Let's the user know how many sticks are in the pile and prompts for stick removal selection
            Console.Out.WriteLine("Pile #" + selectedPile + " has " + Piles[selectedPile - 1].ToString() + " sticks.\n" +
            "Please select a number of sticks to remove from pile #" + selectedPile + ":");
            
            //validates stick selection
            selectedSticks = NimGame.NumberValidate(Console.In.ReadLine(), Piles[selectedPile-1]);
            
            //attemts to remove the sticks from the pile
            if (Piles[selectedPile-1].RemoveSticks(selectedSticks))
            {
                Console.WriteLine(selectedSticks.ToString() + " sticks were removed from pile " + (selectedPile).ToString());
            }
            else
            {
                Console.WriteLine("Check the removal Function");
                Console.ReadLine();
            }
        }

        public override bool IsA(Player N) //For player type comparison
        {
            return this.GetType() == N.GetType();
        }

        public override string ToString() //For printing
        {
            return Name;
        }

        public override void Win(Player loser)//Reward Player for being good
        {
            Console.WriteLine ("____    ____  __    ______ .___________.  ______   .______     ____    ____ \n" +
                                "\\   \\  /   / |  |  /      ||           | /  __  \\  |   _  \\    \\   \\  /   / \n" +
                                " \\   \\/   /  |  | |  ,----'`---|  |----`|  |  |  | |  |_)  |    \\   \\/   /  \n" +
                                "  \\      /   |  | |  |         |  |     |  |  |  | |      /      \\_    _/   \n" +
                                "   \\    /    |  | |  `----.    |  |     |  `--'  | |  |\\  \\----.   |  |     \n" +
                                "    \\__/     |__|  \\______|    |__|      \\______/  | _| `._____|   |__|     \n" );
            base.Win(loser);
            Console.Out.WriteLine("You WIN, contratulations!");
            Console.WriteLine("Press Enter to Continue");
            Console.ReadLine();
        }
    }
}
