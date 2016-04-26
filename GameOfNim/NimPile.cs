/*Program: Game of Nim
 * Programmers: JOhn Shannon, Jason Holdridge, Charles Penzien
 * Assignment: Seminar 6 Group Project
 * Description: Describes a Pile of sticks for use in the game
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

    class NimPile
    {
        //holds the node, done instead of inheritance for easier manipulation of Node
        private Node first;
        public Node First
        {
            get { return first; }
            private set { first = value; }
        }
        private int decRep; //-represents the number of sticks in the pile in decimal form
        


         public NimPile()
        {
            first = null;
            decRep = 0;
        }

        public NimPile(int sticks)
        {
            this.first = new Node(sticks);
            this.decRep = sticks;
        }

        //attempts to remove sticks returns true on success, if it fails, returns false
        public bool RemoveSticks(int sticks)
        {
            if (sticks > decRep || sticks <= 0)
            {
                return false;
            }
            decRep -= sticks;
            first = new Node(decRep);
            return true;
        }


        //adds sticks, as the opposite to Remove sticks
        public void AddSticks(int Sticks)
        {
            decRep += Sticks;
            first = new Node(decRep);
        }


        //Adds Nodes together and keeps track of size
        public static NimPile NimSum(Node n1, Node n2)
        {
            NimPile temp = new NimPile();
            temp.first = n1 + n2;
            temp.decRep = temp.first.ConvertToInt();
            return temp;
        }

        //To print the size of the pile
        public override string ToString()
        {
            return decRep.ToString();
        }


        //returns the integer valur that represents the number of sticks in decimal format
        public int PileSize()
        {
            return decRep;
        }

        //checks that Piles are of the smae size
        public bool Equals(NimPile N)
        {
            return this.decRep == N.decRep;
        }

    }

    class Node
    {
        protected int bit;

        protected Node next;
        protected Node previous; //makes a double linked list 




        #region Constructors


        //default constructor
        public Node()
        {
            this.next = null;
            this.previous = null;
        }

        //constructor that accepts the previous node in order to preserve the link
        public Node(Node previous)
        {
            this.next = null;
            this.previous = previous;
        }

        //Initial constructor to convert the in to binary bits
        public Node(int bit)
        {
            this.bit = bit % 2;

            this.previous = null;
            if (bit / 2 == 0)
            {
                this.next = null;
            }
            else
            {
                this.next = new Node(this, bit / 2);
            }
        }

        //Contructor that takes previous node to maintain the link and the next level of int to be converted
        public Node(Node previous, int bit)
        {
            this.bit = bit % 2;
            this.previous = previous;
            previous.next = this;
            if (bit / 2 == 0)
            {
                this.next = null;
            }
            else
            {
                this.next = new Node(this, (bit / 2) );
            }
        }

        #endregion
        //possible methods Constructor(int) - converts int to binary form



        //GetLast()- travels down the list until the last node is found, returns reference to the last node
        public Node GetLast()
        {
            Node current = this;
            while (current.next != null)
            {
                current = current.next;
            }
            return current;
        }

        //overload + - makes nimSum work better - adds the bits of the binary form list without carrying
        //Essentially an XOR function that also handles inputs of different sizes
        public static Node operator +(Node n1, Node n2)
        {
            if (n1 == null || n2 == null)
            {
                throw new ArgumentNullException();//checks for null references
            }



            Node current1 = n1; //for transversing n1
            Node current2 = n2; //for transversing n2
            Node temp = new Node(); //for storing and returning
            Node currentTemp = temp; //for transversing temporary Node


            while (current1.next != null && current2.next != null) //runs through while both have future Nodes
            {
                currentTemp.bit = (current1.bit + current2.bit) % 2; //(1+1 = 2)%2 = 0. (1+0 = 1)%2 = 1. (0+0 = 0) %2 = 0.
                currentTemp.next = new Node(currentTemp);
                currentTemp = currentTemp.next;
                current1 = current1.next;
                current2 = current2.next;
            }

            if (current1.next == null && current2.next == null) // if both end, grab the add the last nodes and return temp
            {
                currentTemp.bit = (current1.bit + current2.bit) % 2;
                return temp;
            }
            //TODO: Consider refactoring into multiple private function and calls
            currentTemp.bit = (current1.bit + current2.bit) % 2;
            currentTemp.next = new Node(currentTemp);               //grab last node they share
            currentTemp = currentTemp.next;
            current1 = current1.next;
            current2 = current2.next;


            if (current1 == null)  //the next 2 ifs just append the remaining of the unfinished list to the temp list
            {
                while (current2.next != null)
                {
                    currentTemp.bit = current2.bit;
                    currentTemp.next = new Node(currentTemp);
                    currentTemp = currentTemp.next;
                    current2 = current2.next;
                }
                currentTemp.bit = current2.bit;

                return temp;
            }

            if (current2 == null)
            {
                while (current1.next != null)
                {
                    currentTemp.bit = current1.bit;
                    currentTemp.next = new Node(currentTemp);
                    currentTemp = currentTemp.next;
                    current1 = current1.next;
                }
                currentTemp.bit = current1.bit;
                return temp;
            }


            return temp;
        }


        //overload ToString() - possible for error testing or display - Displayes in binary 1,0 form
        public override string ToString()
        {
            string output = "";
            Node current = this.GetLast(); // reading binary buts the largest bit in the front, but converting from decimal places that at the end of the list


            while (current.previous != null) //checks until the next node is void
            {
                output = output + current.bit.ToString();
                current = current.previous;
            }
            output = output + current.bit.ToString();//last node would be skipped if this line did not appear
            return output;
        }


        public int ConvertToInt() //returns the decimal representation of the binary string
        {
            int result = 0;
            int power = 0;
            Node current = this; //used for transversing the list


            while (current.next != null) //runs until the last node
            {
                result += (current.bit * (int)(Math.Pow(2, power)));   //converting using sum of powers of 2 if the current bit is 1
                power++;
                current = current.next; //moves to next
            }
            result += (current.bit * (int)(Math.Pow(2, power)));//picks up last node
            return result;

        }

    }
}
