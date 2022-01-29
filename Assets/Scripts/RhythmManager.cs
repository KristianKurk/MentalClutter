using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager instance;

    public GameObject tilePrefab;
    public GameObject[] shutes;
    public KeyCode[] keyCodes;

    public int currentWordIndex = 0;

    public RhythmData[] rhythms;
    public int currentRhythm;

    public int[] beatToHit;
    public int[] beatToSpawn;
    public int currentBeat;

    public string displayedSentence = "";
    public TMP_Text sentenceDisplayText;
    public string[] missedNoteTexts;

    public bool areAnyGood = false;
    public bool areAnyPressed = false;

    private string selectedNoun, selectedVerb, selectedAdjective, selectedAdverb;
    private int selectedNounValue, selectedVerbValue, selectedAdjectiveValue, selectedAdverbValue;

    public string[] terribleWords;

    private void Awake()
    {

        string[] words = LoremImpsum.Split(' ');
        this.rhythms[rhythms.Length - 1].sequence = new RhythmData.Sequence[words.Length];
        for (int i = 0; i < words.Length; i++)
        {
            this.rhythms[rhythms.Length - 1].sequence[i].word = words[i];
            this.rhythms[rhythms.Length - 1].sequence[i].beat = i;
        }

        instance = this;
        MusicManager.instance.enabled = false;
        Invoke("StartMusic", 1.8f);
    }

    public void StartMusic()
    {
        MusicManager.instance.enabled = true;
        if (GameManager.instance?.words != null)
            SetNewSong(GameManager.instance.level - 1, GameManager.instance.words[0], GameManager.instance.words[1], GameManager.instance.words[2], GameManager.instance.words[3], GameManager.instance.values[0], GameManager.instance.values[1], GameManager.instance.values[2], GameManager.instance.values[3]);
        else
            SetNewSong(rhythms.Length - 1, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, 0);
    }

    public void Update()
    {
        areAnyGood = false;
        areAnyPressed = false;
        foreach (KeyCode key in keyCodes)
        {
            if (Input.GetKeyDown(key))
            {
                areAnyPressed = true;
            }
        }
    }

    public void LateUpdate()
    {
        if (areAnyPressed && !areAnyGood)
        {
            if (GameObject.Find("GameManager"))
            {
                ScoreManager.instance.IncrementFailures();
                SFX.instance.PlayWrongPressSFX();
            }
        }

        sentenceDisplayText.text = displayedSentence;
    }

    public void SetNewSong(int rhythmIndex, string selectedNoun, string selectedVerb, string selectedAdjective, string selectedAdverb, int selectedNounValue, int selectedVerbValue, int selectedAdjectiveValue, int selectedAdverbValue)
    {
        this.currentRhythm = rhythmIndex;
        currentWordIndex = 0;
        currentBeat = 0;

        this.selectedNoun = selectedNoun;
        this.selectedVerb = selectedVerb;
        this.selectedAdjective = selectedAdjective;
        this.selectedAdverb = selectedAdverb;

        this.selectedNounValue = selectedNounValue;
        this.selectedVerbValue = selectedVerbValue;
        this.selectedAdjectiveValue = selectedAdjectiveValue;
        this.selectedAdverbValue = selectedAdverbValue;

        beatToHit = new int[rhythms[currentRhythm].sequence.Length];
        for (int i = 0; i < rhythms[currentRhythm].sequence.Length; i++)
            beatToHit[i] = rhythms[currentRhythm].sequence[i].beat + rhythms[currentRhythm].beatOffset;

        MusicManager.instance.Init(rhythms[currentRhythm].clip, rhythms[currentRhythm].beatsPerMinute, rhythms[currentRhythm].beatsPerMinute, rhythms[currentRhythm].firstBeatOffset);
        CalculateBeatsToSpawn();
    }

    public void TempGoNext()
    {
        int nextSong = currentRhythm + 1;
        if (nextSong >= rhythms.Length)
            nextSong = 0;

        SetNewSong(nextSong, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, 0);
    }

    public void NextBeat()
    {
        if (currentWordIndex < this.rhythms[currentRhythm].sequence.Length)
        {
            if (currentBeat == beatToSpawn[currentWordIndex])
            {
                //Randomly select a shute down which the tile will fall
                int randomShuteIndex = Random.Range(0, shutes.Length);
                GameObject randomShute = shutes[randomShuteIndex];

                GameObject newTile = Instantiate(tilePrefab, randomShute.transform);
                TileFall tileFall = newTile.GetComponent<TileFall>();
                tileFall.points = 1;
                SetText(tileFall);

                if (currentWordIndex == 0)
                    tileFall.word = tileFall.word.ToLowerInvariant();

                tileFall.keyCode = keyCodes[randomShuteIndex];
                tileFall.beatToHit = beatToHit[currentWordIndex];
                tileFall.secondsToFall = this.rhythms[currentRhythm].secondsToFall;
                tileFall.ySpawn = 353;
                tileFall.yEnd = -353;
                tileFall.targetCenter = -100;
                tileFall.beatMargin = 0;

                currentWordIndex++;
            }
        }
        currentBeat++;
    }

    private void SetText(TileFall tile)
    {
        string word = this.rhythms[currentRhythm].sequence[currentWordIndex].word;

        if (word == "*noun*" && selectedNoun != null)
        {
            word = this.selectedNoun.ToLower();
            tile.points = selectedNounValue;
        }
        else if (word == "*verb*" && selectedVerb != null)
        {
            word = this.selectedVerb.ToLower();
            tile.points = selectedVerbValue;
        }
        else if (word == "*adj*" && selectedAdjective != null)
        {
            word = this.selectedAdjective.ToLower();
            tile.points = selectedAdjectiveValue;
        }
        else if (word == "*adv*" && selectedAdverb != null)
        {
            word = this.selectedAdverb.ToLower();
            tile.points = selectedAdverbValue;
        }

        if (word == string.Empty)
        {
            int randomIndex = Random.Range(0, terribleWords.Length);
            word = terribleWords[randomIndex];
        }

        tile.word = word;
        tile.gameObject.GetComponentInChildren<Text>().text = word;
    }

    private void CalculateBeatsToSpawn()
    {
        float beatsPerSec = MusicManager.instance.songBpm / 60;
        float beatsToFall = rhythms[currentRhythm].secondsToFall * beatsPerSec;
        beatToSpawn = new int[rhythms[currentRhythm].sequence.Length];
        for (int i = 0; i < rhythms[currentRhythm].sequence.Length; i++)
        {
            beatToSpawn[i] = beatToHit[i] - (int)beatsToFall;
        }
    }

    private string LoremImpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod magna leo, scelerisque sagittis est sodales ut. Donec vitae nisi iaculis, varius massa et, aliquam purus. Morbi non dui velit. In ultricies vehicula ante, eu hendrerit ipsum lobortis non. Nunc mollis sollicitudin consequat. Maecenas feugiat pharetra tellus, id blandit felis commodo non. Nam pharetra fringilla orci, eu cursus purus varius et. Fusce convallis sit amet ex eget molestie. Mauris a ipsum sit amet urna maximus volutpat sed sit amet orci. Ut elementum ante enim, ac volutpat ligula efficitur in. Nullam vel lectus dapibus, malesuada mi vel, semper magna. Nullam dolor turpis, viverra ut fringilla et, bibendum vel leo. Curabitur sit amet placerat nisi, id elementum metus. Nullam commodo mattis dolor non imperdiet. Nam tristique ultrices magna in congue. Maecenas vel nunc ut odio vehicula interdum. Duis dictum magna vitae mi ullamcorper facilisis. Mauris sagittis elit nulla, sit amet pellentesque massa pulvinar dapibus. Duis ultricies tempus porttitor. Curabitur rhoncus commodo quam, feugiat elementum augue commodo ut. Pellentesque vitae lectus sit amet nisi mattis rutrum. In egestas rhoncus magna. Maecenas commodo, tortor quis tempus blandit, nunc magna auctor dui, vitae faucibus justo est eu metus. In vitae fermentum massa, at efficitur justo. Integer ornare justo non mi sollicitudin dictum. Nulla dictum fermentum ex, sed aliquet risus suscipit vitae. Nulla non imperdiet libero. Donec ultricies libero arcu, a commodo dolor porta vel. Nullam aliquam, massa et molestie faucibus, tellus massa blandit nunc, id congue augue lacus vitae velit. Aenean quis dignissim velit. Suspendisse eget neque augue. Sed pulvinar augue leo, eu porta magna accumsan in. Nam sit amet feugiat libero. Suspendisse efficitur sapien nibh, eu ornare ante efficitur at. Donec rhoncus dui in orci rutrum, a lacinia est gravida. Sed ac viverra justo, faucibus tincidunt ligula. Sed magna arcu, dignissim in velit et, tincidunt facilisis lectus. In hendrerit et mauris id hendrerit. Cras vitae nisi nunc. Aenean malesuada euismod ex vel mollis. Suspendisse quis varius nunc, nec accumsan mauris. Etiam vitae urna at ligula egestas porttitor. Vestibulum erat sapien, sodales sit amet porta ac, mollis sit amet quam. Etiam blandit enim molestie finibus elementum. Fusce ultricies ultricies faucibus. Pellentesque laoreet a ex et consequat. Donec suscipit mattis nisl, eu rutrum leo. Phasellus congue vitae tortor varius molestie. Integer finibus sagittis libero vel molestie. Aenean dapibus felis vel turpis semper, sit amet finibus libero sagittis. Curabitur nunc velit, ornare a consequat id, interdum non purus. Aliquam varius augue quis nibh pellentesque, ut placerat felis luctus. Maecenas tristique lectus euismod viverra aliquet. Suspendisse ornare venenatis augue id luctus. Fusce purus diam, rhoncus ut augue et, suscipit tempus purus. Duis in nunc convallis, rutrum ipsum eu, pulvinar massa. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Nunc tristique, purus non lacinia euismod, orci odio faucibus turpis, eget convallis erat orci at risus. Aliquam erat volutpat. Nullam non eros erat. Ut sed suscipit lorem. Sed quis nunc nec erat pulvinar consequat. Vivamus viverra euismod dui at mattis. Maecenas sollicitudin imperdiet ultricies. Mauris a sagittis justo. Fusce sagittis mi enim, eget bibendum elit posuere vel. Donec quis orci lacinia, dignissim odio eu, pulvinar arcu. Integer imperdiet felis aliquet magna gravida, ultrices tincidunt diam faucibus. Aliquam quis eleifend risus. Maecenas non congue erat, ac tempor justo. Nulla facilisi. Pellentesque ut sapien vel dolor rhoncus mollis. Vestibulum sagittis ex vel erat congue posuere. Donec massa sem, imperdiet ac vehicula vitae, hendrerit in risus. Morbi aliquet venenatis sapien, id cursus tellus ultrices quis. Mauris sagittis ac nisi ac dictum. Proin non rutrum ipsum. Nunc sollicitudin urna id est eleifend, varius consequat nisi ornare. Fusce blandit dolor arcu, in accumsan urna tristique in. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In hac habitasse platea dictumst. Vestibulum at diam placerat, semper diam ut, congue lorem. Phasellus commodo nisi ac consequat pretium. Mauris blandit tempor erat, a suscipit eros aliquet posuere. Maecenas iaculis urna eu luctus sodales. Nam tempus dui non turpis tristique, et accumsan tortor tempus. Suspendisse sed arcu sed elit posuere interdum. Etiam dapibus urna et facilisis mattis. Etiam iaculis augue porta venenatis bibendum. Fusce tristique tortor nunc, hendrerit lacinia ligula vehicula a. Ut pulvinar ac dui et vulputate. Mauris in justo cursus, feugiat odio vel, aliquet nunc. Sed posuere sem vitae nulla suscipit, vulputate pharetra odio finibus. Morbi non ligula dui. Maecenas vel augue pretium mi malesuada tempor. Suspendisse porta laoreet diam eget consequat. Aenean gravida magna tortor, a lobortis urna aliquet sit amet. Nam eu faucibus est, sit amet ullamcorper ipsum. Sed ut iaculis mauris, a feugiat dui. Phasellus semper ante at nisi sodales scelerisque. Sed lorem erat, tincidunt vitae elementum ut, iaculis sit amet orci. Morbi ac felis nec dui ultricies semper. Nunc et sagittis lectus. Donec ac elit vitae sem fermentum faucibus non in diam. Maecenas pharetra dapibus semper. Morbi pretium lectus in purus ornare tincidunt. Donec malesuada magna nec tortor dignissim, id feugiat quam dictum. Quisque ac neque sit amet odio cursus elementum non ut arcu. Nullam nec porttitor justo. Integer faucibus est vitae finibus malesuada. Mauris blandit, mauris a sollicitudin bibendum, tellus libero finibus est, ac porttitor leo neque et magna. Mauris malesuada nibh nec ligula aliquet eleifend. Duis est urna, vehicula eget hendrerit id, gravida quis nisi. Sed maximus varius nulla ac lobortis. Maecenas congue ligula et erat fermentum euismod. Aliquam erat volutpat. Maecenas viverra vulputate nisi vel faucibus. Sed purus nisl, interdum non accumsan et, blandit et ligula. Mauris id bibendum libero. Etiam rutrum est eget sodales ultricies. Sed in enim est. Nunc facilisis faucibus urna et pellentesque. Pellentesque non ornare ipsum, at malesuada tellus. Mauris porta metus sed lectus lacinia commodo. Morbi sit amet dolor malesuada, vulputate mauris at, interdum tellus. Donec eu cursus mauris, in semper nisi. Mauris non elit arcu. Curabitur vel maximus enim. Donec eget urna lobortis, fringilla est in, gravida libero. Curabitur facilisis nec felis ut maximus. Maecenas quis ultricies quam, id luctus massa. Vivamus id sodales augue, ut accumsan nulla. Pellentesque et ullamcorper ipsum. Mauris sem metus, posuere vitae ligula sit amet, laoreet elementum dui. Maecenas ac diam mattis, vestibulum quam eu, mattis lorem. In et odio nunc. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam ut dapibus ipsum, vitae imperdiet odio. Suspendisse et accumsan erat. Curabitur id ex convallis eros facilisis laoreet a non massa. Curabitur molestie augue maximus lectus volutpat viverra. Aenean id tristique tellus, ac bibendum ante. Suspendisse ut volutpat velit, eu dictum ex. Vivamus viverra velit a leo posuere commodo. Sed vitae lacus tincidunt diam dignissim consectetur. Interdum et malesuada fames ac ante ipsum primis in faucibus. Fusce dapibus quam nec risus suscipit, non gravida lacus semper. Morbi elementum leo ut sollicitudin congue. Quisque dolor velit, vulputate sed vulputate a, semper euismod nibh. Aliquam volutpat risus id mauris sagittis faucibus. Curabitur vel tellus at eros suscipit blandit eget quis ex. Donec fermentum ipsum at suscipit volutpat. Fusce vel faucibus orci. Nullam consectetur felis ex, eu tincidunt quam dignissim id. In rhoncus nisi turpis, nec rhoncus libero tempus sodales. Etiam id nunc vitae nulla interdum volutpat. Pellentesque id urna vel velit dictum lobortis ac vel nisl. Duis tincidunt est eu pulvinar faucibus. Cras in vulputate nibh. Praesent in pretium lacus. Maecenas pretium massa ac velit maximus, in sodales urna volutpat. Curabitur a congue lorem. Nunc eget nibh enim. Aliquam imperdiet, sem a vehicula rutrum, leo massa laoreet nisl, vitae ultricies purus ligula malesuada tellus. Curabitur posuere massa sit amet tristique posuere. Sed ex quam, maximus vel ligula nec, efficitur dictum mauris. Fusce consequat, metus sed molestie scelerisque, lectus nisi cursus leo, quis imperdiet nulla urna vitae orci. Sed sit amet ullamcorper dui. Praesent vitae leo urna. Proin scelerisque pretium maximus. Mauris mattis condimentum urna nec tincidunt. Nunc cursus erat a lacus cursus bibendum. Phasellus scelerisque efficitur justo, a vestibulum turpis cursus ac. Curabitur at erat sem. Phasellus eu quam et orci sollicitudin elementum quis quis eros. Phasellus tempus semper sem. Vivamus non sem consequat, tristique turpis eu, fringilla orci. Fusce at iaculis velit. Nullam at tristique leo. Phasellus sit amet gravida libero. Nam nec elit pellentesque, faucibus ligula vel, lobortis risus. Vivamus vel dapibus libero, vitae mollis neque. Mauris volutpat eget dolor sed dignissim. Ut mauris est, pulvinar eu ornare ac, efficitur eget metus. Aliquam dapibus massa id ornare vehicula. Aenean sapien dui, bibendum a aliquet ac, ultricies interdum metus. Maecenas cursus malesuada turpis id rutrum. Duis elementum.";
}
