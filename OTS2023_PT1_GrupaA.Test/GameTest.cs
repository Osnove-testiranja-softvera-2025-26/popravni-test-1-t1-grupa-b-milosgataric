using NUnit.Framework;
using OTS2026_PT1_GrupaA.Exceptions;
using OTS2026_PT1_GrupaA.Models;
using OTS2026_PT1_GrupaB.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_PT1_GrupaA.Test
{

    [TestFixture]
    public class GameTest
    {
        private Game game;

        [SetUp]
        public void SetUp()
        {

            // Igrač i čamac na Land polju (5,5). HasBoat se podrazumeva false
            game = new Game(new Position(5, 5), new Position(5, 5));
        }


        //  F1 - Inicijalizacija igre (konstruktor)



        [TestCase(5, 20)]   
        [TestCase(20, 25)]  
        [TestCase(0, 13)]   
        [TestCase(25, 0)]   
        public void PlayerNotInLand_ThrowsException(int playerX, int playerY)
        {
            Exception ex = Assert.Throws<InvalidPlayerPositionException>((TestDelegate)(
                () => new Game(new Position(playerX, playerY), new Position(5, 5))));
            Assert.That(ex.Message, Is.EqualTo("Player and boat must be in the Land zone!"));
        }

        [TestCase(5, 20)]   
        [TestCase(0, 13)]  
        [TestCase(29, 19)]  
        public void BoatNotInLand_ThrowsException(int boatX, int boatY)
        {
            Exception ex = Assert.Throws<InvalidPlayerPositionException>((TestDelegate)( () => new Game(new Position(5, 5), new Position(boatX, boatY))));
            Assert.That(ex.Message, Is.EqualTo("Player and boat must be in the Land zone!"));

        }

        // Konstruktor indeksira niz pre provere granica -> van granica mape baca IndexOutOfRangeException.
        [TestCase(30, 5)]
        [TestCase(-1, 5)]
        public void PlayerOutsideMap_ThrowsIndexOutOfRange(int playerX, int playerY)
        {
            Assert.Throws<IndexOutOfRangeException>((TestDelegate)(() => new Game(new Position(playerX, playerY), new Position(5, 5))));
        }

        [TestCase(30, 5)]
        [TestCase(5, -1)]
        public void BoatOutsideMap_ThrowsIndexOutOfRange(int boatX, int boatY)
        {
            Assert.Throws<IndexOutOfRangeException>((TestDelegate)(
                () => new Game(new Position(5, 5), new Position(boatX, boatY))));
        }

        [Test]
        public void ValidPositions_PlayerCreated()
        {
            Game g = new Game(new Position(0, 0), new Position(24, 12));
            Assert.That(g.Player, Is.Not.Null);
            Assert.That(g.Player.Position.X, Is.EqualTo(0));
            Assert.That(g.Player.Position.Y, Is.EqualTo(0));
        }



        //  Move Player



        [TestCaseSource(typeof(GameTestCaseData), "MovePlayer_SuccessfulMove_PositionChanged_TestData")]
        public void MovePlayer_SuccessfulMove_PositionChanged(
           int xCoord, int yCoord, Move move, int expectedX, int expectedY)
        {
            game.Player.Position = new Position(xCoord, yCoord);
            game.MovePlayer(move);
            Assert.That(game.Player.Position.X, Is.EqualTo(expectedX));
            Assert.That(game.Player.Position.Y, Is.EqualTo(expectedY));
        }

        [Test]
        public void MovePlayer_InvalidZone_PlayerDoesNotMove()
        {
            game.Player.Position = new Position(5, 12); 
            game.MovePlayer(Move.Down);
            Assert.That(game.Player.Position.X, Is.EqualTo(5));
            Assert.That(game.Player.Position.Y, Is.EqualTo(12));
        }

        [Test]
        public void MovePlayer_OutsideMap_PlayerDoesNotMove()
        {
            game.Player.Position = new Position(0, 0); 
            game.MovePlayer(Move.Up);
            Assert.That(game.Player.Position.X, Is.EqualTo(0));
            Assert.That(game.Player.Position.Y, Is.EqualTo(0));
        }

        [Test]
        public void MovePlayer_IntoPondWithoutBoat_PlayerDoesNotMove()
        {
            game.Player.Position = new Position(15, 19); 
            game.Player.HasBoat = false;
            game.MovePlayer(Move.Down);
            Assert.That(game.Player.Position.Y, Is.EqualTo(19));
        }

        [Test]
        public void MovePlayer_IntoPondWithBoat_PlayerMoves()
        {
            game.Player.Position = new Position(15, 19);
            game.Player.HasBoat = true;
            game.MovePlayer(Move.Down);
            Assert.That(game.Player.Position.X, Is.EqualTo(15));
            Assert.That(game.Player.Position.Y, Is.EqualTo(20));
        }

        //  F2 - ValidatePosition


        [Test]
        public void ValidatePosition_Null_ReturnsFalse()
        {
            Assert.That(game.ValidatePosition(null), Is.EqualTo(false));
        }

        [Test]
        public void ValidatePosition_PondWithBoat_ReturnsTrue()
        {
            game.Player.HasBoat = true;
            Assert.That(game.ValidatePosition(new Position(5, 20)), Is.EqualTo(true));
        }

        [Test]
        public void ValidatePosition_PondWithoutBoat_ReturnsFalse()
        {
            game.Player.HasBoat = false;
            Assert.That(game.ValidatePosition(new Position(5, 20)), Is.EqualTo(false));
        }

        // F3 ResolvePlayerPosition

        [TestCase(5, 5, 0, 1)]
        [TestCase(15, 15, 3, 4)]
        public void ResolvePlayerPosition_Bait_BaitIncreased(
            int xCoord, int yCoord, int initialBait, int expectedBait)
        {
            game.Player.Position = new Position(xCoord, yCoord);
            game.Player.AmountOfBait = initialBait;
            game.Map.Fields[xCoord, yCoord].Content = FieldContent.Bait;
            game.ResolvePlayerPosition();
            Assert.That(game.Player.AmountOfBait, Is.EqualTo(expectedBait));
        }

        [TestCase(5, 5, 2, 1, 1)]
        [TestCase(15, 15, 5, 4, 1)]
        public void ResolvePlayerPosition_FishWithBait_FishCaught(
            int xCoord, int yCoord, int initialBait, int expectedBait, int expectedFish)
        {
            game.Player.Position = new Position(xCoord, yCoord);
            game.Player.AmountOfBait = initialBait;
            game.Map.Fields[xCoord, yCoord].Content = FieldContent.Fish;
            game.ResolvePlayerPosition();
            Assert.That(game.Player.AmountOfBait, Is.EqualTo(expectedBait));
            Assert.That(game.Player.AmountOfFish, Is.EqualTo(expectedFish));
        }

        [Test]
        public void ResolvePlayerPosition_FishWithoutBait_NoCatch()
        {
            game.Player.Position = new Position(5, 5);
            game.Player.AmountOfBait = 0;
            game.Map.Fields[5, 5].Content = FieldContent.Fish;
            game.ResolvePlayerPosition();
            Assert.That(game.Player.AmountOfBait, Is.EqualTo(0));
            Assert.That(game.Player.AmountOfFish, Is.EqualTo(0));
        }

        [Test]
        public void ResolvePlayerPosition_Boat_HasBoatSet()
        {
            game.Player.Position = new Position(5, 5);
            game.Map.Fields[5, 5].Content = FieldContent.Boat;
            game.ResolvePlayerPosition();
            Assert.That(game.Player.HasBoat, Is.EqualTo(true));
        }

        [Test]
        public void ResolvePlayerPosition_Empty_NothingChanged()
        {
            game.Player.Position = new Position(5, 5);
            game.ResolvePlayerPosition();
            Assert.That(game.Player.AmountOfBait, Is.EqualTo(0));
            Assert.That(game.Player.AmountOfFish, Is.EqualTo(0));
            Assert.That(game.Player.HasBoat, Is.EqualTo(false));
        }

        [TestCase(5, 5)]
        [TestCase(15, 15)]
        public void ResolvePlayerPosition_FieldEmptiedAfterResolving(int xCoord, int yCoord)
        {
            game.Player.Position = new Position(xCoord, yCoord);
            game.Map.Fields[xCoord, yCoord].Content = FieldContent.Bait;
            game.ResolvePlayerPosition();
            Assert.That(game.Map.Fields[xCoord, yCoord].Content, Is.EqualTo(FieldContent.Empty));
        }

        // F4 - Calculate Score

        [TestCaseSource(typeof(GameTestDataFromFile), "Get_CalculateIncome_OKInput_SuccesfulCalculation_TestData", new object[] { "calculate_income_data.txt" })]
        public void CalculateIncome_OKInput_SuccesfulCalculation(int fish, int bait, bool hasBoat, Game.Score expectedScore)
        {
            game.Player.AmountOfFish = fish;
            game.Player.AmountOfBait = bait;
            game.Player.HasBoat = hasBoat;
            Game.Score actualScore = game.CalculateIncome();
            Assert.That(actualScore, Is.EqualTo(expectedScore));
        }
    }
}

        
    

       
    
















    









