using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;

public class Talent
{
    public int row, col;
    public string ability;
    public bool inherited, defaultTaken;

    public static void PrintTalent(int spec, int row, int col)
    {
        SetAnchor(25 + (spec == 1 ? 213 : (spec == 2 ? 425 : 0)) + 62 * col, -62 * row - 23);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            var playerClass = currentSave.player.GetClass();
            var talent = playerClass.talentTrees[spec].talents.Find(x => x.row == row && x.col == col);
            var previousTalent = currentSave.player.PreviousTalent(spec, talent);
            var previousTalentDistance = previousTalent == null ? 0 : talent.row - previousTalent.row;
            var abilityObj = Ability.abilities.Find(x => x.name == talent.ability);
            AddBigButton("Ability" + talent.ability.Replace(" ", "").Replace(":", ""),
                (h) =>
                {
                    var canPick = currentSave.player.CanPickTalent(spec, talent);
                    if (!currentSave.player.abilities.Contains(talent.ability) && currentSave.player.CanPickTalent(spec, talent))
                    {
                        currentSave.player.unspentTalentPoints--;
                        PlaySound("DesktopTalentAcquired", 0.2f);
                        currentSave.player.abilities.Add(talent.ability);
                        CDesktop.Rebuild();
                    }
                },
                (h) => () =>
                {
                    SetAnchor(Top, 0, -13);
                    AddHeaderGroup();
                    SetRegionGroupWidth(256);
                    SetRegionGroupHeight(237);
                    AddHeaderRegion(() =>
                    {
                        AddLine(talent.ability, "Gray");
                    });
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("Ability" + talent.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                        if (abilityObj != null)
                        {
                            AddLine("Required level: ", "DarkGray");
                            AddText(1 + "", "Gray");
                            AddLine("Cooldown: ", "DarkGray");
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn" : " turns"), "Gray");
                        }
                    });
                    if (abilityObj != null)
                    {
                        abilityObj.PrintDescription(currentSave.player, null, 256);
                        if (abilityObj.cost != null)
                            foreach (var cost in abilityObj.cost)
                            {
                                AddRegionGroup();
                                AddHeaderRegion(() =>
                                {
                                    AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                                });
                                AddRegionGroup();
                                SetRegionGroupWidth(20);
                                AddHeaderRegion(() =>
                                {
                                    AddLine(cost.Value + "", Board.board != null ? (cost.Value > Board.board.player.resources[cost.Key] ? "Red" : "Green") : "Gray");
                                });
                            }
                    }
                    else AddPaddingRegion(() =>
                    {
                        AddLine("Ability wasn't found!", "Red");
                        SetRegionAsGroupExtender();
                    });
                    AddRegionGroup();
                    SetRegionGroupWidth(256 - (abilityObj == null || abilityObj.cost == null ? 0 : abilityObj.cost.Count) * 49);
                    AddPaddingRegion(() => { AddLine(); });
                }
            );
            if (currentSave.player.abilities.Contains(talent.ability))
                AddBigButtonOverlay("OtherGlowLearned");
            else
            {
                var canPick = currentSave.player.CanPickTalent(spec, talent);
                if (currentSave.player.CanPickTalent(spec, talent)) AddBigButtonOverlay("OtherGlowLearnable");
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
                    body.transform.localScale = new Vector3(1, previousTalentDistance * 18 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 44 : 0), 1);
                    AddBigButtonOverlay("OtherTalentArrowFillHead").GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
                body = AddBigButtonOverlay("OtherTalentArrowBody");
                body.transform.localPosition = new Vector3(0, 28, 0);
                body.transform.localScale = new Vector3(1, previousTalentDistance * 14 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 48 : 0), 1);
                AddBigButtonOverlay("OtherTalentArrowHead");
            }
        });
    }

}
