using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BebasFirstLib.Structs.Impl;
using BebasFirstLib.Algorithms.Impl;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BebasFirstVisualize {
    [RequireComponent(typeof(Visualizer))]
    public class Manager : MonoBehaviour {
        public static Manager Instance { get; private set; }

        [SerializeField]
        Button buttonStartSearch;
        [SerializeField]
        TMP_InputField inputFilepath;
        [SerializeField]
        TMP_Text textResult;
        [SerializeField, Range(0f, 10f)]
        float delay;
        [SerializeField, Range(1, 50)]
        int numIterations;

        Visualizer vis;

        bool processing;
        MazeTreasureMap maze;
        Stopwatch sw;

        public int Mode { get; set; }
        public bool ReturnToStart { get; set; }

        private void Start() {
            if(Instance != null) Destroy(this);
            Instance = this;

            processing = false;
            vis = GetComponent<Visualizer>();
        }

        private void Update() {
            buttonStartSearch.interactable = !processing;
        }

        public void OnStartSearch() {
            if(processing) return;
            if(maze == null) {
                UnityEngine.Debug.LogError(textResult.text);
                return;
            }
            StartCoroutine(Search());
        }

        public void OnLoadFile() {
            string path = inputFilepath.text;
            if(string.IsNullOrEmpty(path)) {
                processing = false;
                return;
            }

            FileStream f = null;
            try {
                f = File.OpenRead(path);
                var stream = new StreamReader(f);

                var size = Helper.VectorFrom(1, 1);
                maze = new MazeTreasureMap(size);
                maze.Read(stream);

                vis.Visualize(maze);
            } catch(FileNotFoundException) {
                UnityEngine.Debug.LogError(textResult.text);
            } finally {
                f?.Close();
            }
        }

        IEnumerator Search() {
            processing = true;
            textResult.text = string.Empty;

            sw = Stopwatch.StartNew();
            sw.Stop(); long _ = sw.ElapsedTicks;
            sw.Reset();

            var steps = new List<MazeTreasureSearchStep>();
            long elapsed = long.MaxValue;
            switch(Mode) {
                case 0: {
                        var currentSteps = new List<MazeTreasureSearchStep>();
                        for(int i = 0; i < numIterations; i++) {
                            currentSteps.Clear();

                            var alg = new MazeTreasureBFS(maze);
                            var trv = new MazeTreasureTraversal<MazeTreasureBFS>(alg, ReturnToStart);
                            sw = Stopwatch.StartNew();
                            foreach(var step in trv.Search(tile => true))
                                currentSteps.Add(step);
                            sw.Stop();

                            long currentElapsed = sw.ElapsedTicks;
                            if(currentElapsed < elapsed) {
                                steps.Clear();
                                steps.AddRange(currentSteps);
                                elapsed = currentElapsed;
                            }
                        }
                        break;
                    }
                case 1: {
                        var currentSteps = new List<MazeTreasureSearchStep>();
                        for(int i = 0; i < numIterations; i++) {
                            currentSteps.Clear();

                            var alg = new MazeTreasureDFS(maze);
                            var trv = new MazeTreasureTraversal<MazeTreasureDFS>(alg, ReturnToStart);
                            sw = Stopwatch.StartNew();
                            foreach(var step in trv.Search(tile => true))
                                currentSteps.Add(step);
                            sw.Stop();

                            long currentElapsed = sw.ElapsedTicks;
                            if(currentElapsed < elapsed) {
                                steps.Clear();
                                steps.AddRange(currentSteps);
                                elapsed = currentElapsed;
                            }
                        }
                        break;
                    }
            }

            vis.ResetColor();
            foreach(var i in steps) {
                vis.Accept(i);
                yield return new WaitForSeconds(delay);
            }

            processing = false;

            double t = (double)elapsed * Stopwatch.Frequency / (1000L * 1000L * 1000L);
            textResult.text = $"Time elapsed: {t}ms Visited nodes: {steps.Count}";
            if(steps.Count > 0 && steps[steps.Count - 1].Found) {
                textResult.text += $" Path length: {steps[steps.Count - 1].Value.Length}";
            }
            UnityEngine.Debug.Log(textResult.text);
        }
    }
}
