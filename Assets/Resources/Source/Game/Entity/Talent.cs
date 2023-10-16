using UnityEngine;

using static Root;
using static Ability;
using static SaveGame;
using static Sound;

public class Talent
{
    //Row and column of the talent in the talent tree
    //There are three columns: 0, 1 and 2
    //There are eleven rows from 0 to 10 with row 10 being reserved for the most powerful abilities
    public int row, col;

    //Name of the ability provided by this talent
    public string ability;

    //Indicates whether the talent requires the previous one in this
    //talent's column to be picked before this one can be picked also
    public bool inherited;

    //Indicates whether this talent is picked by defualt
    //This is generally used for the first talent in every specialisation
    //to make sure that the player will have at least 3 abilities to start off
    public bool defaultTaken;

    //Prints the talent on the screen for it to be picked
    public static void PrintTalent(int tree, int row, int col)
    {
        SetAnchor((tree == 0 ? 147 : -301) + 57 * col, -57 * row + 142);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            var playerSpec = currentSave.player.Spec();
            var talent = playerSpec.talentTrees[tree].talents.Find(x => x.row == row && x.col == col);
            var previousTalent = currentSave.player.PreviousTalent(tree, talent);
            var previousTalentDistance = previousTalent == null ? 0 : talent.row - previousTalent.row;
            var abilityObj = abilities.Find(x => x.name == talent.ability);
            AddBigButton(abilities.Find(x => x.name == talent.ability).icon,
                (h) =>
                {
                    var canPick = currentSave.player.CanPickTalent(tree, talent);
                    if (!currentSave.player.abilities.Contains(talent.ability) && currentSave.player.CanPickTalent(tree, talent))
                    {
                        currentSave.player.unspentTalentPoints--;
                        PlaySound("DesktopTalentAcquired", 0.2f);
                        currentSave.player.abilities.Add(talent.ability);
                        CDesktop.RebuildAll();
                    }
                },
                null,
                (h) => () =>
                {
                    PrintAbilityTooltip(currentSave.player, null, abilities.Find(x => x.name == talent.ability));
                }
            );
            if (currentSave.player.abilities.Contains(talent.ability))
                AddBigButtonOverlay("OtherGlowLearned");
            else
            {
                var canPick = currentSave.player.CanPickTalent(tree, talent);
                if (currentSave.player.CanPickTalent(tree, talent)) AddBigButtonOverlay("OtherGlowLearnable");
                else
                {
                    SetBigButtonToGrayscale();
                    AddBigButtonOverlay("OtherGridBlurred");
                }
            }
            if (talent.inherited)
            {
                GameObject body = null;
                if (currentSave.player.abilities.Contains(previousTalent.ability))
                {
                    body = AddBigButtonOverlay("OtherTalentArrowFillBody");
                    body.transform.localPosition = new Vector3(0, 26, 0);
                    body.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    body.transform.localScale = new Vector3(1, previousTalentDistance * 13 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 44 : 0), 1);
                    AddBigButtonOverlay("OtherTalentArrowFillHead").GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
                body = AddBigButtonOverlay("OtherTalentArrowBody");
                body.transform.localPosition = new Vector3(0, 28, 0);
                body.transform.localScale = new Vector3(1, previousTalentDistance * 9 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 48 : 0), 1);
                AddBigButtonOverlay("OtherTalentArrowHead");
            }
        });
    }
}
