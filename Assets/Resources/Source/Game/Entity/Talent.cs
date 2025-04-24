using UnityEngine;

using static Root;
using static Ability;
using static SaveGame;
using static Sound;
using System.Linq;

public class Talent
{
    //Row and column of the talent in the talent tree
    //There are three columns: 0, 1 and 2
    //There are five rows from 0 to 4 with row 4 being reserved for the most powerful abilities
    public int row, col;

    //Index of the tree, for each specialisation there are two trees
    //The first one marked with 0 is the Novice tree, the second one
    //marked with a 1 is the Adept tree that is unlocked after 50%
    //of the Novice tree has been unlocked on a character
    public int tree;

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
    public static Talent PrintTalent(int spec, int row, int col, int tree)
    {
        var playerSpec = currentSave.player.Spec();
        var talent = playerSpec.talentTrees[spec].talents.Find(x => x.row == row && x.col == col && x.tree == tree);
        SetAnchor((tree == 0 ? -301 : 147) + 57 * col, -57 * row + 142);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            var previousTalent = currentSave.player.PreviousTalent(spec, talent);
            var previousTalentDistance = previousTalent == null ? 0 : talent.row - previousTalent.row;
            var abilityObj = abilities.Find(x => x.name == talent.ability);
            AddBigButton(abilityObj.icon,
                (h) =>
                {
                    if (currentSave.player.unspentTalentPoints > 0 && currentSave.player.CanPickTalent(spec, talent))
                    {
                        currentSave.player.unspentTalentPoints--;
                        PlaySound("DesktopTalentAcquired", 0.4f);
                        if (currentSave.player.abilities.ContainsKey(talent.ability))
                            currentSave.player.abilities[talent.ability]++;
                        else currentSave.player.abilities.Add(talent.ability, 0);
                        CDesktop.RespawnAll();
                    }
                },
                (h) =>
                {
                    if (currentSave.player.abilities.ContainsKey(talent.ability))
                    {
                        if (talent.defaultTaken && currentSave.player.abilities[talent.ability] == 0) return;
                        var talentsPicked = playerSpec.talentTrees[spec].talents.Where(x => x != talent && currentSave.player.abilities.ContainsKey(x.ability)).ToList();
                        currentSave.player.abilities[talent.ability]--;
                        if (currentSave.player.abilities[talent.ability] < 0)
                            currentSave.player.abilities.Remove(talent.ability);
                        var canUndoTalentPick = true;
                        foreach (var talentPick in talentsPicked)
                            if (!currentSave.player.CanPickTalent(spec, talentPick, true))
                                canUndoTalentPick = false;
                        if (!canUndoTalentPick)
                            if (currentSave.player.abilities.ContainsKey(talent.ability))
                                currentSave.player.abilities[talent.ability]++;
                            else currentSave.player.abilities.Add(talent.ability, 0);
                        else
                        {
                            PlaySound("DesktopCantClick", 0.4f);
                            currentSave.player.unspentTalentPoints++;
                            CDesktop.RespawnAll();
                        }
                    }
                },
                (h) => () => PrintAbilityTooltip(currentSave.player, abilities.Find(x => x.name == talent.ability), currentSave.player.abilities.ContainsKey(talent.ability) ? (currentSave.player.abilities[talent.ability] == abilities.Find(x => x.name == talent.ability).ranks.Count - 1 ? currentSave.player.abilities[talent.ability] : currentSave.player.abilities[talent.ability] + 1) : 0)
            );
            var canPick = currentSave.player.CanPickTalent(spec, talent);
            if (currentSave.player.abilities.ContainsKey(talent.ability) && !canPick)
            {
                if (!canPick) AddBigButtonOverlay("OtherGlowLearned");
            }
            else
            {
                if (canPick && currentSave.player.unspentTalentPoints > 0)
                    AddBigButtonOverlay("OtherGlowLearnable");
                else
                {
                    SetBigButtonToGrayscale();
                    AddBigButtonOverlay("OtherGridBlurred");
                }
            }
            if (talent.inherited)
            {
                GameObject body = null;
                var ab = currentSave.player.abilities.ContainsKey(previousTalent.ability) ? currentSave.player.abilities[previousTalent.ability] : -1;
                if (ab >= 0)
                {
                    body = AddBigButtonOverlay("OtherTalentArrowFillBody" + (ab < abilities.Find(x => x.name == previousTalent.ability).ranks.Count - 1 ? "Green" : ""));
                    body.transform.localPosition = new Vector3(0, 26, 0);
                    body.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    body.transform.localScale = new Vector3(1, previousTalentDistance * 13 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 44 : 0), 1);
                    AddBigButtonOverlay("OtherTalentArrowFillHead" + (ab < abilities.Find(x => x.name == previousTalent.ability).ranks.Count - 1 ? "Green" : "")).GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
                body = AddBigButtonOverlay("OtherTalentArrowBody");
                body.transform.localPosition = new Vector3(0, 28, 0);
                body.transform.localScale = new Vector3(1, previousTalentDistance * 9 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 48 : 0), 1);
                AddBigButtonOverlay("OtherTalentArrowHead");
            }
        });
        return talent;
    }
}
