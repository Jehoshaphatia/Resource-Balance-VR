using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing
{
    public class AudioManager : MonoBehaviour
    {
        public GvrAudioSource gvrAudioSource;
        public AudioClip mineMountainAudioClip;
        public AudioClip mineTreesAudioClip;
        public AudioClip buttonClickClip;
        public AudioClip constructionSoundsClip;
        public AudioClip plantingTreesClip;
        [Range(0f, 1f)] public float volume = 1f;

        public enum AudioEvent
        {
            MiningMountain,
            MiningTrees,
            Construction,
            PlantingTrees,
            ButtonClick
        }

        public void PlayAudio(AudioEvent audioEvent)
        {
            AudioClip clip;

            switch (audioEvent)
            {
                case AudioEvent.MiningMountain: clip = mineMountainAudioClip; break;
                case AudioEvent.MiningTrees: clip = mineTreesAudioClip; break;
                case AudioEvent.Construction: clip = constructionSoundsClip; break;
                case AudioEvent.PlantingTrees: clip = plantingTreesClip; break;
                case AudioEvent.ButtonClick: clip = buttonClickClip; break;
                default:
                    throw new System.Exception("Unrecognized AudioEvent " + audioEvent);
            }

            gvrAudioSource.PlayOneShot(clip, volume);
        }

        public void PlayAudio(PlayerMode playerMode, BuildOption? buildOption, TileType? tileType = null)
        {
			AudioEvent audioEvent;

			switch (playerMode)
			{
				case PlayerMode.Building:
					if (!buildOption.HasValue)
						throw new System.ArgumentNullException("buildOption");

					switch (buildOption)
					{
						case BuildOption.City:
						case BuildOption.PowerPlant:
							audioEvent = AudioEvent.Construction;
							break;

						default:
							audioEvent = AudioEvent.PlantingTrees;
							break;
					}
					break;

				case PlayerMode.Mining:
					if (!tileType.HasValue)
						throw new System.ArgumentNullException("tileType");

					switch (tileType)
					{
						case TileType.Trees:
							audioEvent = AudioEvent.MiningTrees;
							break;

						case TileType.Mountain:
							audioEvent = AudioEvent.MiningMountain;
							break; 

						case TileType.City:
						case TileType.PowerPlant:
							audioEvent = AudioEvent.Construction;
							break;

						default:
							throw new System.ArgumentException("Unexpected tile type");
					}
					break;

				default:
					audioEvent = AudioEvent.ButtonClick; // acknowledge click regardless
					break;
			}

			PlayAudio(audioEvent);
        }

		public void PlayButtonClick()
		{
			PlayAudio(AudioEvent.ButtonClick);
		}

    }
}