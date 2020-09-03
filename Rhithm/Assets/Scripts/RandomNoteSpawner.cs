﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class RandomNoteSpawner : MonoBehaviour
{

    public float BPM;
    public float secsPerBeat;
    public float difficultyMultiplier; // Adjusts the Spawn Rate, default is 1
    public bool firstSpawn = true;

    // Position of Note Spawns
    public Vector3[] noteSpawnPositions; // Array for spawn positions of notes
    public Vector3 noteOneSpawn; // Spawn Position of Note One
    public Vector3 noteTwoSpawn; // Spawn Position of Note Two
    public Vector3 noteThreeSpawn; // Spawn Position of Note Three

    // Note Objects
    public GameObject noteOne; // Note One Prefab
    public GameObject noteTwo; // Note Two Prefab
    public GameObject noteThree; // Note Three Prefab
    public GameObject obstacle; // Obstacle Game Object

    //public Song song; // The Audio Song object being passed from the menu
    public AudioSource currentSong; // The Song being played
    public float songLength; // The Length of the song being played
    public float currentPlayedTime = 0; // How Long the song has currently been playing for
    public float startDelay; // Delay of spawning for songs that don't start instantly 

    public Score score;

    // Start is called before the first frame update
    void Start()
    {
        //BPM = currentSong.getBPM();
        currentSong = GetComponent<AudioSource>();
        songLength = currentSong.clip.length; // Gets the song's length in seconds
        secsPerBeat = 60f / BPM; // Calculates Seconds per Beat
        noteSpawnPositions = new Vector3[] { noteOneSpawn, noteTwoSpawn, noteThreeSpawn };
        //startDelay = currentSong.getStartDelay();
        //currentSong.play();
        StartCoroutine(SpawnNote()); // Starts spawning Method
    }

    void Update()
    {
        currentPlayedTime += Time.deltaTime; // Ensures accurate timekeeping
    }

    public void createNote(GameObject note, Vector3 spawnPosition)
    {
        Instantiate(note, spawnPosition, Quaternion.identity);
    }

    IEnumerator SpawnNote()
    {
        yield return new WaitForSeconds(startDelay);

        while (songLength - currentPlayedTime > 2.5f) // change this to "Hey while theres more than X seconds of song left, keep spawning"
        {

            float randomNum = UnityEngine.Random.Range(0.0f, 1.0f);

            if (firstSpawn) // Ensures a note spawns on the first beat, rather than a gap or an obstacle
            {
                firstSpawn = false;
                
                if (randomNum >= 0 && randomNum < 0.33)
                {// Spawns Note 1
                    createNote(noteOne, noteSpawnPositions[0]);
                }
                else if (randomNum >= 0.33 && randomNum < 0.66)
                { // Spawns Note 2
                    createNote(noteTwo, noteSpawnPositions[1]);
                }
                else if (randomNum >= 0.66 && randomNum <= 1)
                { // Spawns Note 3
                    createNote(noteThree, noteSpawnPositions[2]);
                }
            } 

            else // Standard Spawning Method
            {
                yield return new WaitForSeconds(secsPerBeat / difficultyMultiplier);
                
                if (randomNum >= 0 && randomNum <= 0.25)
                { // Spawns Note 1
                    createNote(noteOne, noteSpawnPositions[0]);
                }
                else if (randomNum >= 0.30 && randomNum <= 0.55)
                { // Spawns Note 2
                    createNote(noteTwo, noteSpawnPositions[1]);
                }
                else if (randomNum >= 0.60 && randomNum <= 0.85)
                { // Spawns Note 3
                    createNote(noteThree, noteSpawnPositions[2]);
                }
                else if (randomNum >= 0.87 && randomNum < 0.95)
                { // Spawns Obstacle
                    int index = UnityEngine.Random.Range(0, noteSpawnPositions.Length);
                    Vector3 currPos = noteSpawnPositions[index];
                    currPos.y += 0.7f; // Increases the spawn height so that the Obstacle is not in the ground.
                    createNote(obstacle, currPos);
                }
            }
        }

        if(score.getNoteMissed() == false)
        {
            yield return new WaitForSeconds(5.5f); // Waits for celebration!

            //Celebrate here
            Debug.Log("Woop");
        }
    }

   











}