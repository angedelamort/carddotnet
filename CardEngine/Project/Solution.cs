using System;
using System.Collections.Generic;
using Bam;
using CardEngine.Data;

namespace CardEngine.Project
{
    // TODO: add a guid and a name. maybe even versions...
    /// <summary>
    /// This is the data of the application.  You will need the Engine in order to execute this code.
    /// </summary>
    public class Solution
    {
        private readonly List<Card> cards = new List<Card> {new Card {Id = 0, Name = "home"}};

        public Dictionary<int, Script> GlobalScripts { get; } = new Dictionary<int, Script>();

        public Dictionary<string, Variable> GlobalVariables { get; } = new Dictionary<string, Variable>();

        // TODO: When creating the engine, make sure to call Bundle while compiling the scripts.
        public AssetManager AssetManager { get; set; }

        public IReadOnlyList<Card> Cards => cards;

        public Card AddCard()
        {
            var card = new Card {Id = GenerateCardId()};
            cards.Add(card);
            return card;
        }

        public int StartingCardId { get; } = 0;

        private int uniqueCardId = 1;

        private int GenerateCardId() => uniqueCardId++;

        public string ToJson()
        {
            //var t = new ResourceWriter();
            //t.AddResource();

            throw new NotImplementedException();
        }

        public static Solution FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
