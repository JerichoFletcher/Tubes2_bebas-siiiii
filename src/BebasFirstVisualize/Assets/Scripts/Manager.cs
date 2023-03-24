using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using BebasFirstLib.Algorithms.Impl;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using System.Text;

namespace BebasFirstVisualize {
    [RequireComponent(typeof(Visualizer))]
    public class Manager : MonoBehaviour {
        public static Manager Instance { get; private set; }

        [SerializeField]
        Button buttonStartSearch, buttonLoadFile, buttonShowPath;
        [SerializeField]
        TMP_InputField inputFilepath;
        [SerializeField]
        TMP_Text textResult;
        [SerializeField]
        TMP_Dropdown dropdownAlgo;
        [SerializeField]
        Toggle toggleTSP;
        [SerializeField, Range(0f, 10f)]
        float delay;
        [SerializeField, Range(1, 50)]
        int numIterations;

        Visualizer vis;

        MazeTreasureMap maze;
        Stopwatch sw;

        public int Mode { get; set; }
        public bool ReturnToStart { get; set; }

        bool Processing {
            get {
                return _processing;
            }
            set {
                _processing = value;
                inputFilepath.interactable
                    = buttonLoadFile.interactable
                    = dropdownAlgo.interactable
                    = toggleTSP.interactable
                    = !value;
            }
        }
        bool _processing;

        private void Start() {
            if(Instance != null) Destroy(this);
            Instance = this;

            Processing = false;
            vis = GetComponent<Visualizer>();

            textResult.text = string.Empty;
        }

        private void Update() {
            buttonShowPath.interactable = vis.HasPath;
            buttonStartSearch.interactable = maze != null && !Processing;
        }

        public void OnStartSearch() {
            if(Processing) return;
            if(maze == null) {
                UnityEngine.Debug.LogError(textResult.text);
                return;
            }
            StopCoroutines();
            StartCoroutine(Search());
        }

        public void OnLoadFile() {
            string path = inputFilepath.text;
            if(string.IsNullOrEmpty(path))
                return;

            FileStream f = null;
            StreamReader stream = null;
            try {
                f = File.OpenRead(path);
                stream = new StreamReader(f);

                var size = Helper.VectorFrom(1, 1);
                var newMaze = new MazeTreasureMap(size);
                newMaze.Read(stream);
                maze = newMaze;

                StopCoroutines();
                vis.Visualize(maze);
                
                textResult.text = "";
                UnityEngine.Debug.Log($"Successfully read from file {path}");
            } catch(FileNotFoundException) {
                textResult.text = "File not found!";
                UnityEngine.Debug.LogError($"Not found: {path}");
            } catch(InvalidDataException) {
                textResult.text = "File is invalid!";
                UnityEngine.Debug.LogError($"Invalid: {path}");
            } catch(Exception e) {
                textResult.text = "Failed to open!";
                UnityEngine.Debug.LogError($"Caught {e.GetType().Name}: {path}\n{e}");
            } finally {
                stream?.Close();
                f?.Close();
            }
        }

        public void OnExit() {
            StopCoroutines();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        IEnumerator Search() {
            Processing = true;
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

            vis.ResetDisplay();
            foreach(var i in steps) {
                vis.Accept(i);
                yield return new WaitForSeconds(delay);
            }

            Processing = false;

            double t = (double)elapsed * Stopwatch.Frequency / (1000L * 1000L * 1000L);
            textResult.text = $"Time elapsed: {t}ms Visited nodes: {steps.Count}";
            if(steps.Count > 0 && steps[^1].Found) {
                textResult.text += $"\nPath (len: {steps[^1].Value.Length - 1}): {GetSteps(steps[^1].Value)}";
            } else {
                textResult.text += " No path found";
            }
            UnityEngine.Debug.Log(textResult.text);
        }

        string GetSteps(Tree<MazeTreasureMap.MapTile>[] nodes) {
            var str = new StringBuilder();
            if(nodes?.Length > 1) {
                for(int i = 0; i < nodes.Length - 1; i++) {
                    IVector<int>
                        v1 = nodes[i].Value.Position,
                        v2 = nodes[i + 1].Value.Position;
                    Vector2Int
                        vec1 = new(v1[0], v1[1]),
                        vec2 = new(v2[0], v2[1]),
                        d = vec2 - vec1;

                    // Because the map is rotated 90 degrees CW from its abstract interpretation
                    if(d.Equals(Vector2Int.left))
                        str.Append('U');
                    else if(d.Equals(Vector2Int.right))
                        str.Append('D');
                    else if(d.Equals(Vector2Int.up))
                        str.Append('R');
                    else if(d.Equals(Vector2Int.down))
                        str.Append('L');
                    else
                        str.Append('?');
                }
            }
            return str.ToString();
        }

        void StopCoroutines() {
            StopAllCoroutines();
            vis.StopAllCoroutines();
        }
    }
}
