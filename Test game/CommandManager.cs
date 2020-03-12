using System;
using System.Text;
using Microsoft.Xna.Framework;
using Test_game.Entities;

namespace Test_game
{
    /// <summary>
    /// Manages commands to the enitites
    /// Combat and Entity movement
    /// </summary>
    public class CommandManager
    {
        private Random Dice = new Random(); // this will be the dice used for combat

        //Move the actor BY +/- X&Y coords
        //return true if move was successful 
        //and false otherwise
        public bool MoveActorBy(Actor actor, Point postion)
        {
            return actor.MoveBy(postion);
        }

        // Executes an attack from an attacking actor
        // on a defending actor, and then describes
        // the outcome of the attack in the Message Log
        public void Attack(Actor attacker, Actor defender)
        {
            //Creates 2 message for the outcomes of the attack and defense
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            //Count up the amount fo attacking damage done
            //and the number of successful blocks
            int hits = ResolveAttack(attacker, defender, attackMessage);
            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);
            int damage = 0;
            //Displaye the outcome of the attack & defense
            GameLoop.GameUIManager.MessageLogWindow.Add(attackMessage.ToString());
            //checks if the defender was able to defend
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                GameLoop.GameUIManager.MessageLogWindow.Add(defenseMessage.ToString());
            }

            damage = hits - blocks;

            //Defender takes damage
            ResolveDamage(defender, damage);

        }

        // Calculates the outcome of an attacker's attempt
        // at scoring a hit on a defender, using the attacker's
        // AttackChance and a random number(Dice) as the basis
        // Modifies a StringBuilder message that will be displayed
        // in the MessageLog
        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;
            int diceOutcome = 0;
            Random Dice = new Random(); // this will be the dice used for combat

            attackMessage.AppendFormat("{0} attack {1}", attacker.Name, defender.Name);

            //the attackers attack value determines the amount of times 
            //the dice is rolled
            for (int numDice = 0; numDice < attacker.Attack; numDice++)
            {
                //Rolls a single dice and stores the outcome
                diceOutcome = Dice.Next(1, 100);

                //the attack has to roll at least their attack chance value 
                //or higher in order to register a hit
                //eg. if attack chance is 20
                //the attacker has to roll 20 or higher to score a hit
                if (diceOutcome >= attacker.AttackChance)
                {
                    hits++;
                }

            }
            return hits;
        }

        // Calculates the outcome of a defender's attempt
        // at blocking incoming hits.
        // Modifies a StringBuilder messages that will be displayed
        // in the MessageLog, expressing the number of hits blocked.
        private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;
            int diceOutcome = 0;
            Random Dice = new Random(); // this will be the dice used for combat

            if (hits > 0)
            {
                attackMessage.AppendFormat(", scoring {0} hits.", hits);
                defenseMessage.AppendFormat("{0} defends ", defender.Name);

                for (int numDice = 0; numDice < hits; numDice++)
                {
                    diceOutcome = Dice.Next(1, 100);

                    if (diceOutcome >= defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.AppendFormat(" and blocks {0} of those hits.", blocks);
            }
            else
            {
                attackMessage.AppendFormat(" and misses completely.");
            }
            return blocks;
        }

        // Calculates the damage a defender takes after a successful hit
        // and subtracts it from its Health
        // Then displays the outcome in the MessageLog.
        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                GameLoop.GameUIManager.MessageLogWindow.Add($"{defender.Name} takes {damage} points of damage");
                defender.Health = defender.Health - damage;

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                GameLoop.GameUIManager.MessageLogWindow.Add($"{defender.Name} blocked all damage!");
            }
        }

        //Removes the actor that has died
        //and updates the message log
        private static void ResolveDeath(Actor defender)
        {
            GameLoop.GameUIManager.MessageLogWindow.Add($" {defender.Name} was killed.");
            GameLoop.World.CurrentMap.Remove(defender);
            //GameLoop.GameUIManager.Update();
        }
    }
}
