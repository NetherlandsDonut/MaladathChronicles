using UnityEngine;

using static Root;
using static Serialization;

public class Starter : MonoBehaviour
{
    void Start()
    {
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        testText.Set("Aqualung");
        testText2.Set("Baby Yoda");
        cursor = FindObjectOfType<Cursor>();
        Board.board = new Board(8, 8);
        SpawnDesktopBlueprint("Menu");

        Data.data = new Data()
        {
            unitCards = new System.Collections.Generic.List<UnitCard>(),
            spellCards = new System.Collections.Generic.List<SpellCard>(),
            leaderCards = new System.Collections.Generic.List<LeaderCard>()
            {
                new LeaderCard()
                {
                    title = "Lord Mazdamundi",
                    race = Race.Lizardmen
                },
                new LeaderCard()
                {
                    title = "Kroq-Gar",
                    race = Race.Lizardmen
                },
                new LeaderCard()
                {
                    title = "Tiktaq\'To",
                    race = Race.Lizardmen
                },
                new LeaderCard()
                {
                    title = "Thorgrim Grudgebearer",
                    race = Race.Dwarfs
                },
                new LeaderCard()
                {
                    title = "Ungrim Ironfist",
                    race = Race.Dwarfs
                },
                new LeaderCard()
                {
                    title = "Baltasar Gelt",
                    race = Race.TheEmpire
                },
                new LeaderCard()
                {
                    title = "Karl Franz",
                    race = Race.TheEmpire
                },
                new LeaderCard()
                {
                    title = "Malus Darkblade",
                    race = Race.DarkElves
                },
                new LeaderCard()
                {
                    title = "Rakarth",
                    race = Race.DarkElves
                },
                new LeaderCard()
                {
                    title = "Isabela Von Carstein",
                    race = Race.VampireCounts
                },
            }
        };
        //Serialize(Data.data, "Data");

        Destroy(gameObject);
    }
}
