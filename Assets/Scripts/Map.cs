using UnityEngine;
using System.Collections.Generic;
using System;


public struct map_position_properties
{
    public float height;
    public int next_direction; //-1 = left, -2 = bottom, 1 = right, 2 = top
    public int old_direction; //-1 = left, -2 = bottom, 1 = right, 2 = top
    public bool end;
    public bool jump;
} 

public struct lvl_position
{
    public int pos_x;
    public int pos_z;
    public float height;
    public int tile_object; //0 = nothing, 1 = coin, 2 = spike , 3 = barrote, 4 = trampolin, 5 = bird
    public bool jump;
    public string bird_direction;

}


public class Map : MonoBehaviour
{
    private map_position_properties [,] map_positions;
    public static Map Instance; // Singleton Instance
    private bool finished, keep_destroying;
    private lvl_position [,] lv1;
    private lvl_position [,] lv2;
    private List <GameObject> cubes;
    private List <GameObject> last_cubes;
    private List <GameObject> pinxos;
    private List <GameObject> barrotes;
    private List <GameObject> coins;
    private List <GameObject> trampolines;
    int current_stage, n_stages, cube_done, n_cubes;
    private Coin my_coin;
    private LoadBird my_bird;
    private Pinxo my_pinxo;
    private Game my_game;
    private Trampolin my_trampolin;
    private Barrote my_barrote;
    private Vector2 Last_pos;
    private  GameObject ship1, ship2;
    private  GameObject shark1, shark2, shark3, shark4;

    

    private void Awake()
    {
         if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        my_coin = FindObjectOfType<Coin>();
        my_pinxo = FindObjectOfType<Pinxo>();
        my_barrote = FindObjectOfType<Barrote>();
        my_bird = FindObjectOfType<LoadBird>();
        my_trampolin = FindObjectOfType<Trampolin>();
        my_game = Game.Instance;
        cubes = new List<GameObject>();
        pinxos = new List<GameObject>();
        trampolines = new List<GameObject>();
        barrotes = new List<GameObject>();
        last_cubes = new List<GameObject>();
        coins = new List<GameObject>();
        map_positions = new map_position_properties [10,10];
        keep_destroying = false;
        lv1 = new lvl_position[,]{
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f , tile_object = 1 , jump = true},
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f , tile_object = 2},
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f , tile_object = 5},
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 7, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 9, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 1, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 2, height = 1.0f , tile_object = 2},
            new lvl_position { pos_x = 3, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f , tile_object = 2},
            new lvl_position { pos_x = 5, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 8, pos_z = 4, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 8, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 1, height = 1.0f , tile_object = 5, bird_direction = "Right"},
            new lvl_position { pos_x = 8, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , tile_object = 5, bird_direction = "Left"},
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f , tile_object = 1 , jump = true},
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f , jump = true},
            new lvl_position { pos_x = 1, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 7, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 1, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f , tile_object = 1 , jump = true},
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 2.0f , jump = true},
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 7, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 9, pos_z = 5, height = 1.0f }, 
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 6, height = 1.0f , jump = true},
            new lvl_position { pos_x = 1, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 6, height = 2.0f },
            new lvl_position { pos_x = 3, pos_z = 6, height = 2.0f , tile_object = 5},
            new lvl_position { pos_x = 4, pos_z = 6, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 7, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 7, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 1, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 1, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f  },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f, tile_object = 1 , jump = true},
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f , tile_object = 5},
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 8, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 8, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 2.0f , tile_object = 1},
            new lvl_position { pos_x = 6, pos_z = 6, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 5, height = 2.0f, jump = true},
            new lvl_position { pos_x = 6, pos_z = 4, height = 2.0f  },
            new lvl_position { pos_x = 6, pos_z = 3, height = 3.0f },
            new lvl_position { pos_x = 6, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f , jump = true},
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 2.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 5, height = 2.0f , tile_object = 1 },
            new lvl_position { pos_x = 5, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 5, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 1, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 1, height = 2.0f , tile_object = 1 },
            new lvl_position { pos_x = 6, pos_z = 1, height = 2.0f , tile_object = 1 },
            new lvl_position { pos_x = 7, pos_z = 1, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 2, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 4, height = 2.0f , jump = true},
            new lvl_position { pos_x = 7, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 6, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 3.0f },
            new lvl_position { pos_x = 7, pos_z = 8, height = 3.0f },
            new lvl_position { pos_x = 7, pos_z = 9, height = 3.0f }},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 1, pos_z = 4, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 3, pos_z = 2, height = 2.0f },
            new lvl_position { pos_x = 3, pos_z = 1, height = 2.0f, tile_object = 5 },
            new lvl_position { pos_x = 4, pos_z = 1, height = 2.0f },
            new lvl_position { pos_x = 5, pos_z = 1, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 1, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 8, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 8, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 5, height = 1.0f , jump = true},
            new lvl_position { pos_x = 8, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 8, pos_z = 8, height = 2.0f , tile_object = 1 },
            new lvl_position { pos_x = 7, pos_z = 8, height = 2.0f , tile_object = 1 },
            new lvl_position { pos_x = 6, pos_z = 8, height = 2.0f , tile_object = 1 , jump = true},
            new lvl_position { pos_x = 5, pos_z = 8, height = 2.0f },
            new lvl_position { pos_x = 4, pos_z = 8, height = 3.0f },
            new lvl_position { pos_x = 3, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 8, height = 1.0f }},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 1, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 3, height = 2.0f , tile_object = 5},
            new lvl_position { pos_x = 6, pos_z = 4, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 7, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 2.0f , jump = true},
            new lvl_position { pos_x = 8, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 9, pos_z = 7, height = 3.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 6, height = 1.0f , tile_object = 1 , jump = true},
            new lvl_position { pos_x = 0, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 7, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 2, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 5, pos_z = 7, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 5, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 2, pos_z = 4, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 3, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 2, height = 1.0f , tile_object = 2 },
            new lvl_position { pos_x = 6, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 8, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 9, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}}
    };
        lv2 = new lvl_position[,]{
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f , tile_object = 4},
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f , tile_object = 5},
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 7, pos_z = 5, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 8, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 9, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 5, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 3, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 5, pos_z = 2, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 5, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 2.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 6, height = 2.0f, tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 8, height = 2.0f  , tile_object = 5},
            new lvl_position { pos_x = 0, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 9, height = 1.0f , jump = true},
            new lvl_position { pos_x = 3, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 7, height = 1.0f , tile_object = 4},
            new lvl_position { pos_x = 3, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 4, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 8, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 6, pos_z = 9, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 2.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 6, height = 2.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 8, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 9, height = 3.0f },
            new lvl_position { pos_x = 1, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 9, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 3, pos_z = 9, height = 1.0f},
            new lvl_position { pos_x = 4, pos_z = 9, height = 1.0f , tile_object = 5},
            new lvl_position { pos_x = 4, pos_z = 8, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 6, height = 2.0f },
            new lvl_position { pos_x = 4, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 2.0f },
            new lvl_position { pos_x = 4, pos_z = 3, height = 2.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 3, height = 3.0f },
            new lvl_position { pos_x = 7, pos_z = 3, height = 3.0f },
            new lvl_position { pos_x = 7, pos_z = 4, height = 3.0f , jump = true},
            new lvl_position { pos_x = 7, pos_z = 5, height = 3.0f },
            new lvl_position { pos_x = 7, pos_z = 6, height = 3.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 4.0f , tile_object = 1},
            new lvl_position { pos_x = 7, pos_z = 8, height = 4.0f , tile_object = 1},
            new lvl_position { pos_x = 7, pos_z = 9, height = 4.0f , tile_object = 1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f  , jump = true},
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f , tile_object = 2},
            new lvl_position { pos_x = 1, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 4, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 6, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 6, pos_z = 7, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 6, pos_z = 8, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 8, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 2, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 8, height = 1.0f , tile_object = 5},
            new lvl_position { pos_x = 0, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 1, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 4, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 1, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 1, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 2, height = 2.0f , tile_object = 5, bird_direction = "Right"},
            new lvl_position { pos_x = 6, pos_z = 3, height = 2.0f , tile_object = 1},
            new lvl_position { pos_x = 6, pos_z = 4, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 7, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 7, height = 1.0f,   tile_object = 3},
            new lvl_position { pos_x = 3, pos_z = 8, height = 1.0f ,},
            new lvl_position { pos_x = 3, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 4, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 2.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 6, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 8, height = 3.0f },
            new lvl_position { pos_x = 0, pos_z = 9, height = 3.0f , tile_object = 1},
            new lvl_position { pos_x = 1, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 9, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 3, pos_z = 9, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 9, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 9, height = 2.0f },
            new lvl_position { pos_x = 5, pos_z = 8, height = 2.0f },
            new lvl_position { pos_x = 5, pos_z = 7, height = 2.0f , tile_object = 5, bird_direction = "Right"},
            new lvl_position { pos_x = 5, pos_z = 6, height = 2.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 5, pos_z = 4, height = 3.0f },
            new lvl_position { pos_x = 5, pos_z = 3, height = 1.0f , tile_object = 2},
            new lvl_position { pos_x = 5, pos_z = 2, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 5, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 4, height = 1.0f , tile_object = 2},
            new lvl_position { pos_x = 2, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 1, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 5, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 1, height = 1.0f , tile_object = 1},
            new lvl_position { pos_x = 7, pos_z = 1, height = 1.0f , jump = true},
            new lvl_position { pos_x = 7, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 4, height = 1.0f , tile_object = 3},
            new lvl_position { pos_x = 7, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 8, height = 1.0f , jump = true},
            new lvl_position { pos_x = 6, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 8, height = 2.0f },
            new lvl_position { pos_x = 4, pos_z = 8, height = 2.0f , tile_object = 1},
            new lvl_position { pos_x = 3, pos_z = 8, height = 2.0f , tile_object = 1},
            new lvl_position { pos_x = 2, pos_z = 8, height = 2.0f , jump = true},
            new lvl_position { pos_x = 1, pos_z = 8, height = 2.0f },
            new lvl_position { pos_x = 0, pos_z = 8, height = 3.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 1, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 3, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 4, height = 2.0f , tile_object = 5},
            new lvl_position { pos_x = 6, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 3, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 7, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 7, pos_z = 7, height = 2.0f , jump = true},
            new lvl_position { pos_x = 8, pos_z = 7, height = 2.0f },
            new lvl_position { pos_x = 9, pos_z = 7, height = 3.0f },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}},
            {
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 3, height = 1.0f , jump = true},
            new lvl_position { pos_x = 0, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 5, height = 1.0f , tile_object = 3  },
            new lvl_position { pos_x = 0, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 0, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 7, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 2, pos_z = 7, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 3, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 7, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 5, pos_z = 7, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 5, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 5, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 3, pos_z = 4, height = 1.0f , jump = true},
            new lvl_position { pos_x = 2, pos_z = 4, height = 1.0f , tile_object = 3 },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 3, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 4, pos_z = 2, height = 1.0f , jump = true},
            new lvl_position { pos_x = 5, pos_z = 2, height = 1.0f , tile_object = 4 },
            new lvl_position { pos_x = 6, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 7, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 8, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = 9, pos_z = 2, height = 1.0f , tile_object = 1 },
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1},
            new lvl_position { pos_x = -1}}

        };


    }
    public bool player_hit(Vector3 player_position)
    {
       return player_position.z <  (map_positions[(int)player_position.x,(int)player_position.y].height - 0.2); 
    }

    private void add_cube_to_position_l1(lvl_position pos){ 
        Texture cubeTexture_grass = Resources.Load<Texture>("Grass");
        Texture cubeTexture_dirt = Resources.Load<Texture>("Dirt");
         Material dynamicMaterial_grass = new Material(Shader.Find("HDRP/Lit"));
        Material dynamicMaterial_dirt = new Material(Shader.Find("HDRP/Lit"));  
        dynamicMaterial_grass.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_dirt.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_grass.mainTexture = cubeTexture_grass;
        dynamicMaterial_dirt.mainTexture = cubeTexture_dirt;
        for (int i = -3; i < pos.height; ++i){
            cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
            cubes[cubes.Count - 1].transform.position = new Vector3(pos.pos_x,i - 5, pos.pos_z);
            if (i == pos.height - 1) cubes[cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_grass;
            else cubes[cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_dirt;
        }
    }

    private void create_cubes_last_l1(Vector3 pos, float last_height){
        Texture cubeTexture_grass = Resources.Load<Texture>("Grass");
        Texture cubeTexture_dirt = Resources.Load<Texture>("Dirt");
        Material dynamicMaterial_grass = new Material(Shader.Find("HDRP/Lit"));
        Material dynamicMaterial_dirt = new Material(Shader.Find("HDRP/Lit"));  
        dynamicMaterial_grass.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_dirt.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_grass.mainTexture = cubeTexture_grass;
        dynamicMaterial_dirt.mainTexture = cubeTexture_dirt;
        for (int i = -3; i < last_height; ++i){
            last_cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
            last_cubes[last_cubes.Count - 1].transform.position = new Vector3(pos.x,i, pos.z);
            if (i == last_height - 1) last_cubes[last_cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_grass;
            else last_cubes[last_cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_dirt;
        }
    }

    private void add_cube_to_position_l2(lvl_position pos){ 
        Texture cubeTexture_wood = Resources.Load<Texture>("wood");
        Texture cubeTexture_stone = Resources.Load<Texture>("stone");
         Material dynamicMaterial_wood = new Material(Shader.Find("HDRP/Lit"));
        Material dynamicMaterial_stone = new Material(Shader.Find("HDRP/Lit"));  
        dynamicMaterial_wood.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_stone.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_wood.mainTexture = cubeTexture_wood;
        dynamicMaterial_stone.mainTexture = cubeTexture_stone;
        for (int i = -3; i < pos.height; ++i){
            cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
            cubes[cubes.Count - 1].transform.position = new Vector3(pos.pos_x,i - 5, pos.pos_z);
            if (i == pos.height - 1) cubes[cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_wood;
            else cubes[cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_stone;
        }
    }

    private void create_cubes_last_l2(Vector3 pos, float last_height){
        Texture cubeTexture_wood = Resources.Load<Texture>("wood");
        Texture cubeTexture_stone = Resources.Load<Texture>("stone");
         Material dynamicMaterial_wood = new Material(Shader.Find("HDRP/Lit"));
        Material dynamicMaterial_stone = new Material(Shader.Find("HDRP/Lit"));  
        dynamicMaterial_wood.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_stone.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_wood.mainTexture = cubeTexture_wood;
        dynamicMaterial_stone.mainTexture = cubeTexture_stone;
        for (int i = -3; i < last_height; ++i){
            last_cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
            last_cubes[last_cubes.Count - 1].transform.position = new Vector3(pos.x,i, pos.z);
            if (i == last_height - 1) last_cubes[last_cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_wood;
            else last_cubes[last_cubes.Count - 1].GetComponent<Renderer>().material = dynamicMaterial_stone;
        }
    }


    private void spawn_objects(){
        string modelPath = "ship"; 
        Vector3 spawnPosition = new Vector3(15, -2, 15);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        GameObject model = Resources.Load<GameObject>(modelPath);
        if (ship1 != null){
            Destroy(ship1);
            Destroy(ship2);
        }
        ship1 = Instantiate(model, spawnPosition, spawnRotation);
        spawnPosition = new Vector3(-5, -2, -5);
        ship2 = Instantiate(model, spawnPosition, spawnRotation);
        ship1.transform.localScale = new Vector3(1.3f,1.3f,1.3f);
        ship2.transform.localScale = new Vector3(1.3f,1.3f,1.3f);
        modelPath = "Shark"; 
        spawnRotation = Quaternion.Euler(0, 0, 0);
        model = Resources.Load<GameObject>(modelPath);
        if (shark1 != null){
            Destroy(shark1);
            Destroy(shark2);
            Destroy(shark3);
            Destroy(shark4);
        }
        spawnPosition = new Vector3(12, -7.5f, 12);
        shark1 = Instantiate(model, spawnPosition, spawnRotation);
        shark1.transform.localScale = new Vector3(0.3f,0.3f,0.3f);

        spawnPosition = new Vector3(-5, -7.5f, 11);
        shark2 = Instantiate(model, spawnPosition, spawnRotation); 
        shark2.transform.localScale = new Vector3(0.3f,0.3f,0.3f);

        spawnPosition = new Vector3(-11, -7.5f, 5);
        shark3 = Instantiate(model, spawnPosition, spawnRotation);  
        shark3.transform.localScale = new Vector3(0.3f,0.3f,0.3f);

        spawnPosition = new Vector3(13, -7.5f, -1);
        shark4 = Instantiate(model, spawnPosition, spawnRotation);  
        shark4.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
    }



    public void load_lvl_stage(int level, int lvl_stage){    
        finished = false;
        Last_pos = new Vector2(0,-1);
        for (int i = 0; i < cubes.Count; ++i){
            Destroy(cubes[i]);
        } 
        for (int i = 0; i < 10; ++i){
            for (int j = 0; j < 10; ++j){
                map_positions[i,j].height = 0;
                map_positions[i,j].old_direction = 0;
                map_positions[i,j].next_direction = 0;
            }
        }
        for (int i = 0; i < pinxos.Count; ++i){
            Destroy(pinxos[i]);
        }
        for (int i = 0; i < last_cubes.Count; ++i){
            Destroy(last_cubes[i]);
        } 
        for (int i = 0; i < trampolines.Count; ++i){
            Destroy(trampolines[i]);
        }        
        for (int i = 0; i < coins.Count; ++i){
            Destroy(coins[i]);
        } 
        for (int i = 0; i < barrotes.Count; ++i){
            Destroy(barrotes[i]);
        } 
        while (my_bird.GetBirdsON().Count != 0){
            my_bird.RemoveBirdAt(0);
        }  
        barrotes.Clear();
        trampolines.Clear(); 
        last_cubes.Clear(); 
        pinxos.Clear();
        coins.Clear();
        cubes.Clear();
        spawn_objects();
        if (level == 1){
            current_stage = lvl_stage;
            n_stages = lv1.GetLength(0);
            cube_done = 0;
            n_cubes = 0;
            add_cube_to_position_l1(lv1[0, 0]);
            int last_pos = lv1.GetLength(1) - 1;
            map_positions[0,0].height = lv1[lvl_stage,0].height;
            map_positions[0,0].old_direction = 2;
            map_positions[0,0].next_direction = (lv1[lvl_stage,0].pos_x != lv1[lvl_stage,1].pos_x) ?  lv1[lvl_stage,1].pos_x - lv1[lvl_stage,0].pos_x   : (lv1[lvl_stage,1].pos_z - lv1[lvl_stage,0].pos_z)*2;
            ++n_cubes;
            if (lv1[lvl_stage,0].tile_object == 1){
                my_coin.CreateCoin(new Vector3(lv1[lvl_stage,0].pos_x,lv1[lvl_stage,0].height, lv1[lvl_stage,0].pos_z));
            }           
            for (int i = 1; i <  last_pos; ++i){
                if (lv1[lvl_stage,i].pos_x == -1){
                    map_positions[lv1[lvl_stage,i - 1].pos_x,lv1[lvl_stage,i - 1].pos_z].end = true;
                    if (lv1[lvl_stage,i - 1].pos_x != lv1[lvl_stage,i - 2].pos_x){
                        if (lv1[lvl_stage,i - 1].pos_x == 0){
                            create_cubes_last_l1( new Vector3(-1, 0, lv1[lvl_stage,i - 1].pos_z ), lv1[lvl_stage,i - 1].height);
                            map_positions[lv1[lvl_stage,i - 1].pos_x,lv1[lvl_stage,i - 1].pos_z].next_direction = -1;
                        }
                        else {
                            create_cubes_last_l1( new Vector3(10, 0, lv1[lvl_stage,i - 1].pos_z ), lv1[lvl_stage,i - 1].height);
                            map_positions[lv1[lvl_stage,i - 1].pos_x,lv1[lvl_stage,i - 1].pos_z].next_direction = 1;
                        }
                    }
                    else {
                        if (lv1[lvl_stage,i - 1].pos_z == 0){
                            create_cubes_last_l1( new Vector3( lv1[lvl_stage,i - 1].pos_x, 0, -1 ), lv1[lvl_stage,i - 1].height);
                            map_positions[lv1[lvl_stage,i - 1].pos_x,lv1[lvl_stage,i - 1].pos_z].next_direction = -2;
                        }
                        else {
                            create_cubes_last_l1( new Vector3(lv1[lvl_stage,i - 1].pos_x, 0, 10 ), lv1[lvl_stage,i - 1].height);
                            map_positions[lv1[lvl_stage,i - 1].pos_x,lv1[lvl_stage,i - 1].pos_z].next_direction = 2;
                        }

                    }
                    return;
                }
                add_cube_to_position_l1(lv1[lvl_stage,i]);
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].old_direction = (lv1[lvl_stage,i].pos_x != lv1[lvl_stage,i - 1].pos_x) ?  lv1[lvl_stage,i].pos_x - lv1[lvl_stage,i - 1].pos_x   : (lv1[lvl_stage,i].pos_z - lv1[lvl_stage,i - 1].pos_z)*2;
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].next_direction = (lv1[lvl_stage,i].pos_x != lv1[lvl_stage,i + 1].pos_x) ?  lv1[lvl_stage,i + 1].pos_x - lv1[lvl_stage,i].pos_x   : (lv1[lvl_stage,i + 1].pos_z - lv1[lvl_stage,i].pos_z)*2;
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].height = lv1[lvl_stage,i].height;
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].jump = lv1[lvl_stage,i].jump;
                map_positions[lv1[lvl_stage,i - 1].pos_x,lv1[lvl_stage,i - 1].pos_z].end = false;
                ++n_cubes;

            }

            if (lv1[lvl_stage,last_pos].pos_x == -1){
                    map_positions[lv1[lvl_stage,last_pos - 1].pos_x,lv1[lvl_stage,last_pos - 1].pos_z].end = true;
                    if (lv1[lvl_stage,last_pos - 1].pos_x != lv1[lvl_stage,last_pos - 2].pos_x){
                        if (lv1[lvl_stage,last_pos - 1].pos_x == 0){
                            create_cubes_last_l1( new Vector3(-1, 0, lv1[lvl_stage,last_pos - 1].pos_z ), lv1[lvl_stage,last_pos - 1].height);
                            map_positions[lv1[lvl_stage,last_pos - 1].pos_x,lv1[lvl_stage,last_pos - 1].pos_z].next_direction = -1;
                        }
                        else {
                            create_cubes_last_l1( new Vector3(10, 0, lv1[lvl_stage,last_pos - 1].pos_z ), lv1[lvl_stage,last_pos - 1].height);
                            map_positions[lv1[lvl_stage,last_pos - 1].pos_x,lv1[lvl_stage,last_pos - 1].pos_z].next_direction = 1;
                        }
                    }
                    else {
                        if (lv1[lvl_stage,last_pos - 1].pos_z == 0){
                            create_cubes_last_l1( new Vector3( lv1[lvl_stage,last_pos - 1].pos_x, 0, -1 ), lv1[lvl_stage,last_pos - 1].height);
                            map_positions[lv1[lvl_stage,last_pos - 1].pos_x,lv1[lvl_stage,last_pos - 1].pos_z].next_direction = -2;
                        }
                        else {
                            create_cubes_last_l1( new Vector3(lv1[lvl_stage,last_pos - 1].pos_x, 0, 10 ), lv1[lvl_stage,last_pos - 1].height);
                            map_positions[lv1[lvl_stage,last_pos - 1].pos_x,lv1[lvl_stage,last_pos - 1].pos_z].next_direction = 2;
                        }

                    }
                    return;
            }
            add_cube_to_position_l1(lv1[lvl_stage,last_pos]);
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].old_direction = (lv1[lvl_stage,last_pos].pos_x != lv1[lvl_stage,last_pos - 1].pos_x) ? lv1[lvl_stage,last_pos].pos_x - lv1[lvl_stage,last_pos - 1].pos_x   : (lv1[lvl_stage,last_pos].pos_z - lv1[lvl_stage,last_pos - 1].pos_z)*2;
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].next_direction =  (lv1[lvl_stage,last_pos].pos_x != lv1[lvl_stage,last_pos - 1].pos_x) ? lv1[lvl_stage,last_pos].pos_x - lv1[lvl_stage,last_pos - 1].pos_x   : (lv1[lvl_stage,last_pos].pos_z - lv1[lvl_stage,last_pos - 1].pos_z)*2;
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].height = lv1[lvl_stage,last_pos].height;
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].end = true;
            if (lv1[lvl_stage,last_pos].pos_x != lv1[lvl_stage,last_pos - 1].pos_x){
                if (lv1[lvl_stage,last_pos].pos_x == 0){
                    create_cubes_last_l1( new Vector3(-1, 0, lv1[lvl_stage,last_pos].pos_z ), lv1[lvl_stage,last_pos].height);
                    map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].next_direction = -1;
                }
                else {
                    create_cubes_last_l1( new Vector3(10, 0, lv1[lvl_stage,last_pos].pos_z ), lv1[lvl_stage,last_pos].height);
                    map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].next_direction = 1;
                }
            }
            else {
                if (lv1[lvl_stage,last_pos].pos_z == 0){
                    create_cubes_last_l1( new Vector3( lv1[lvl_stage,last_pos].pos_x, 0,last_pos -1 ), lv1[lvl_stage,last_pos].height);
                    map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].next_direction = -2;
                }
                else {
                    create_cubes_last_l1( new Vector3(lv1[lvl_stage,last_pos].pos_x, 0, 10 ), lv1[lvl_stage,last_pos].height);
                    map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].next_direction = 2;
                }
            }
            ++n_cubes;
         }
         
        else if (level == 2){
            current_stage = lvl_stage;
            n_stages = lv2.GetLength(0);
            cube_done = 0;
            n_cubes = 0;
            add_cube_to_position_l2(lv2[0, 0]);
            int last_pos = lv2.GetLength(1) - 1;
            map_positions[0,0].height = lv2[lvl_stage,0].height;
            map_positions[0,0].old_direction = 2;
            map_positions[0,0].next_direction = (lv2[lvl_stage,0].pos_x != lv2[lvl_stage,1].pos_x) ?  lv2[lvl_stage,1].pos_x - lv2[lvl_stage,0].pos_x   : (lv2[lvl_stage,1].pos_z - lv2[lvl_stage,0].pos_z)*2;
            ++n_cubes;
            if (lv2[lvl_stage,0].tile_object == 1){
                my_coin.CreateCoin(new Vector3(lv2[lvl_stage,0].pos_x,lv2[lvl_stage,0].height, lv2[lvl_stage,0].pos_z));
            }           
            for (int i = 1; i <  last_pos; ++i){
                if (lv2[lvl_stage,i].pos_x == -1){
                    map_positions[lv2[lvl_stage,i - 1].pos_x,lv2[lvl_stage,i - 1].pos_z].end = true;
                    if (lv2[lvl_stage,i - 1].pos_x != lv2[lvl_stage,i - 2].pos_x){
                        if (lv2[lvl_stage,i - 1].pos_x == 0){
                            create_cubes_last_l2( new Vector3(-1, 0, lv2[lvl_stage,i - 1].pos_z ), lv2[lvl_stage,i - 1].height);
                            map_positions[lv2[lvl_stage,i - 1].pos_x,lv2[lvl_stage,i - 1].pos_z].next_direction = -1;
                        }
                        else {
                            create_cubes_last_l2( new Vector3(10, 0, lv2[lvl_stage,i - 1].pos_z ), lv2[lvl_stage,i - 1].height);
                            map_positions[lv2[lvl_stage,i - 1].pos_x,lv2[lvl_stage,i - 1].pos_z].next_direction = 1;
                        }
                    }
                    else {
                        if (lv2[lvl_stage,i - 1].pos_z == 0){
                            create_cubes_last_l2( new Vector3( lv2[lvl_stage,i - 1].pos_x, 0, -1 ), lv2[lvl_stage,i - 1].height);
                            map_positions[lv2[lvl_stage,i - 1].pos_x,lv2[lvl_stage,i - 1].pos_z].next_direction = -2;
                        }
                        else {
                            create_cubes_last_l2( new Vector3(lv2[lvl_stage,i - 1].pos_x, 0, 10 ), lv2[lvl_stage,i - 1].height);
                            map_positions[lv2[lvl_stage,i - 1].pos_x,lv2[lvl_stage,i - 1].pos_z].next_direction = 2;
                        }

                    }
                    return;
                }
                add_cube_to_position_l2(lv2[lvl_stage,i]);
                map_positions[lv2[lvl_stage,i].pos_x,lv2[lvl_stage,i].pos_z].old_direction = (lv2[lvl_stage,i].pos_x != lv2[lvl_stage,i - 1].pos_x) ?  lv2[lvl_stage,i].pos_x - lv2[lvl_stage,i - 1].pos_x   : (lv2[lvl_stage,i].pos_z - lv2[lvl_stage,i - 1].pos_z)*2;
                map_positions[lv2[lvl_stage,i].pos_x,lv2[lvl_stage,i].pos_z].next_direction = (lv2[lvl_stage,i].pos_x != lv2[lvl_stage,i + 1].pos_x) ?  lv2[lvl_stage,i + 1].pos_x - lv2[lvl_stage,i].pos_x   : (lv2[lvl_stage,i + 1].pos_z - lv2[lvl_stage,i].pos_z)*2;
                map_positions[lv2[lvl_stage,i].pos_x,lv2[lvl_stage,i].pos_z].height = lv2[lvl_stage,i].height;
                map_positions[lv2[lvl_stage,i].pos_x,lv2[lvl_stage,i].pos_z].jump = lv2[lvl_stage,i].jump;
                map_positions[lv2[lvl_stage,i - 1].pos_x,lv2[lvl_stage,i - 1].pos_z].end = false;
                ++n_cubes;

            }

            if (lv2[lvl_stage,last_pos].pos_x == -1){
                    map_positions[lv2[lvl_stage,last_pos - 1].pos_x,lv2[lvl_stage,last_pos - 1].pos_z].end = true;
                    if (lv2[lvl_stage,last_pos - 1].pos_x != lv2[lvl_stage,last_pos - 2].pos_x){
                        if (lv2[lvl_stage,last_pos - 1].pos_x == 0){
                            create_cubes_last_l2( new Vector3(-1, 0, lv2[lvl_stage,last_pos - 1].pos_z ), lv2[lvl_stage,last_pos - 1].height);
                            map_positions[lv2[lvl_stage,last_pos - 1].pos_x,lv2[lvl_stage,last_pos - 1].pos_z].next_direction = -1;
                        }
                        else {
                            create_cubes_last_l2( new Vector3(10, 0, lv2[lvl_stage,last_pos - 1].pos_z ), lv2[lvl_stage,last_pos - 1].height);
                            map_positions[lv2[lvl_stage,last_pos - 1].pos_x,lv2[lvl_stage,last_pos - 1].pos_z].next_direction = 1;
                        }
                    }
                    else {
                        if (lv2[lvl_stage,last_pos - 1].pos_z == 0){
                            create_cubes_last_l2( new Vector3( lv2[lvl_stage,last_pos - 1].pos_x, 0, -1 ), lv2[lvl_stage,last_pos - 1].height);
                            map_positions[lv2[lvl_stage,last_pos - 1].pos_x,lv2[lvl_stage,last_pos - 1].pos_z].next_direction = -2;
                        }
                        else {
                            create_cubes_last_l2( new Vector3(lv2[lvl_stage,last_pos - 1].pos_x, 0, 10 ), lv2[lvl_stage,last_pos - 1].height);
                            map_positions[lv2[lvl_stage,last_pos - 1].pos_x,lv2[lvl_stage,last_pos - 1].pos_z].next_direction = 2;
                        }

                    }
                    return;
            }
            add_cube_to_position_l2(lv2[lvl_stage,last_pos]);
            map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].old_direction = (lv2[lvl_stage,last_pos].pos_x != lv2[lvl_stage,last_pos - 1].pos_x) ? lv2[lvl_stage,last_pos].pos_x - lv2[lvl_stage,last_pos - 1].pos_x   : (lv2[lvl_stage,last_pos].pos_z - lv2[lvl_stage,last_pos - 1].pos_z)*2;
            map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].next_direction =  (lv2[lvl_stage,last_pos].pos_x != lv2[lvl_stage,last_pos - 1].pos_x) ? lv2[lvl_stage,last_pos].pos_x - lv2[lvl_stage,last_pos - 1].pos_x   : (lv2[lvl_stage,last_pos].pos_z - lv2[lvl_stage,last_pos - 1].pos_z)*2;
            map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].height = lv2[lvl_stage,last_pos].height;
            map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].end = true;
            if (lv2[lvl_stage,last_pos].pos_x != lv2[lvl_stage,last_pos - 1].pos_x){
                if (lv2[lvl_stage,last_pos].pos_x == 0){
                    create_cubes_last_l2( new Vector3(-1, 0, lv2[lvl_stage,last_pos].pos_z ), lv2[lvl_stage,last_pos].height);
                    map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].next_direction = -1;
                }
                else {
                    create_cubes_last_l2( new Vector3(10, 0, lv2[lvl_stage,last_pos].pos_z ), lv2[lvl_stage,last_pos].height);
                    map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].next_direction = 1;
                }
            }
            else {
                if (lv2[lvl_stage,last_pos].pos_z == 0){
                    create_cubes_last_l2( new Vector3( lv2[lvl_stage,last_pos].pos_x, 0,last_pos -1 ), lv2[lvl_stage,last_pos].height);
                    map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].next_direction = -2;
                }
                else {
                    create_cubes_last_l2( new Vector3(lv2[lvl_stage,last_pos].pos_x, 0, 10 ), lv2[lvl_stage,last_pos].height);
                    map_positions[lv2[lvl_stage,last_pos].pos_x,lv2[lvl_stage,last_pos].pos_z].next_direction = 2;
                }

            }
            ++n_cubes;
            





         }
    } 

    public bool generating_map(int lvl, int stage){
        for (int i = 0; i < cubes.Count; ++i){
            cubes[i].transform.position = new Vector3 (cubes[i].transform.position.x, cubes[i].transform.position.y + 2.0f * Time.deltaTime, cubes[i].transform.position.z);
        }
        shark1.transform.position = new Vector3 (shark1.transform.position.x, shark1.transform.position.y + 2.0f * Time.deltaTime, shark1.transform.position.z);
        shark2.transform.position = new Vector3 (shark2.transform.position.x, shark2.transform.position.y + 2.0f * Time.deltaTime, shark2.transform.position.z);
        shark3.transform.position = new Vector3 (shark3.transform.position.x, shark3.transform.position.y + 2.0f * Time.deltaTime, shark3.transform.position.z);
        shark4.transform.position = new Vector3 (shark4.transform.position.x, shark4.transform.position.y + 2.0f * Time.deltaTime, shark4.transform.position.z);
        if (cubes[0].transform.position.y >= -3){
            for (int i = 0; i < cubes.Count; ++i){
                cubes[i].transform.position = new Vector3 (cubes[i].transform.position.x, (float)(int)cubes[i].transform.position.y, cubes[i].transform.position.z);
            }
            if (lvl == 1){
                for (int i = 0; i < lv1.GetLength(1); ++i){
                    if (lv1[stage,i].tile_object == 1){
                        coins.Add(my_coin.CreateCoin(new Vector3(lv1[stage,i].pos_x,lv1[stage,i].height, lv1[stage,i].pos_z)));
                    }
                    else if (lv1[stage,i].tile_object == 2){
                        pinxos.Add(my_pinxo.CreatePinxo(new Vector3(lv1[stage,i].pos_x,lv1[stage,i].height, lv1[stage,i].pos_z),false,""));
                    }
                    if (i > 1 && lv1[stage,i].height > 1 && lv1[stage,i - 1].height < lv1[stage,i].height){
                        if (map_positions[lv1[stage,i].pos_x,lv1[stage,i].pos_z].old_direction == 1) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv1[stage,i].pos_x - 0.6f, lv1[stage,i].height - 1.1f, lv1[stage,i].pos_z), true, "LEFT"));
                        else if (map_positions[lv1[stage,i].pos_x,lv1[stage,i].pos_z].old_direction == -1) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv1[stage,i].pos_x + 0.6f, lv1[stage,i].height - 1.1f, lv1[stage,i].pos_z), true, "RIGHT"));
                        else if (map_positions[lv1[stage,i].pos_x,lv1[stage,i].pos_z].old_direction == 2) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv1[stage,i].pos_x , lv1[stage,i].height - 1.1f, lv1[stage,i].pos_z - 0.6f), true, "FRONT"));
                        else if (map_positions[lv1[stage,i].pos_x,lv1[stage,i].pos_z].old_direction == -2) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv1[stage,i].pos_x ,  lv1[stage,i].height - 1.1f, lv1[stage,i].pos_z + 0.6f), true, "BACK"));
                    }
                    if (lv1[stage,i].tile_object == 3){
                        barrotes.Add(my_barrote.CreateBarrote(new Vector3(lv1[stage,i].pos_x,lv1[stage,i].height, lv1[stage,i].pos_z)));
                    }
                    if (lv1[stage,i].tile_object == 4){
                        trampolines.Add(my_trampolin.CreateTrampolin(new Vector3(lv1[stage,i].pos_x,lv1[stage,i].height, lv1[stage,i].pos_z)));
                    }
                    if (lv1[stage,i].tile_object == 5){
                        my_bird.LoadBirdModels(new Vector3(lv1[stage,i].pos_x,lv1[stage,i].height, lv1[stage,i].pos_z),lv1[stage,i].bird_direction);
                    }
                    
                }
            }
            else {
                for (int i = 0; i < lv2.GetLength(1); ++i){
                    if (lv2[stage,i].tile_object == 1){
                        coins.Add(my_coin.CreateCoin(new Vector3(lv2[stage,i].pos_x,lv2[stage,i].height, lv2[stage,i].pos_z)));
                    }
                    else if (lv2[stage,i].tile_object == 2){
                        pinxos.Add(my_pinxo.CreatePinxo(new Vector3(lv2[stage,i].pos_x,lv2[stage,i].height, lv2[stage,i].pos_z),false,""));
                    }
                     if (i > 1 && lv2[stage,i].height > 1 && lv2[stage,i - 1].height < lv2[stage,i].height){
                        if (map_positions[lv2[stage,i].pos_x,lv2[stage,i].pos_z].old_direction == 1) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv2[stage,i].pos_x - 0.6f, lv2[stage,i].height - 1.1f, lv2[stage,i].pos_z), true, "LEFT"));
                        else if (map_positions[lv2[stage,i].pos_x,lv2[stage,i].pos_z].old_direction == -1) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv2[stage,i].pos_x + 0.6f, lv2[stage,i].height - 1.1f, lv2[stage,i].pos_z), true, "RIGHT"));
                        else if (map_positions[lv2[stage,i].pos_x,lv2[stage,i].pos_z].old_direction == 2) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv2[stage,i].pos_x , lv2[stage,i].height - 1.1f, lv2[stage,i].pos_z - 0.6f), true, "FRONT"));
                        else if (map_positions[lv2[stage,i].pos_x,lv2[stage,i].pos_z].old_direction == -2) pinxos.Add(my_pinxo.CreatePinxo( new Vector3(lv2[stage,i].pos_x ,  lv2[stage,i].height - 1.1f, lv2[stage,i].pos_z + 0.6f), true, "BACK"));
                    }
                    if (lv2[stage,i].tile_object == 3){
                        barrotes.Add(my_barrote.CreateBarrote(new Vector3(lv2[stage,i].pos_x,lv2[stage,i].height, lv2[stage,i].pos_z)));
                    }
                    else if (lv2[stage,i].tile_object == 4){
                        trampolines.Add(my_trampolin.CreateTrampolin(new Vector3(lv2[stage,i].pos_x,lv2[stage,i].height, lv2[stage,i].pos_z)));
                    }
                    if (lv2[stage,i].tile_object == 5){
                        my_bird.LoadBirdModels(new Vector3(lv2[stage,i].pos_x,lv2[stage,i].height, lv2[stage,i].pos_z),lv2[stage,i].bird_direction);
                    }
                    
                }
            }
            return true;
        }
        return false; 
    }


    private bool destroying(){
        for (int i = 0; i < pinxos.Count; ++i){
            Destroy(pinxos[i]);
        } 
        pinxos.Clear();
        for (int i = 0; i < coins.Count; ++i){
            Destroy(coins[i]);
        } 
        for (int i = 0; i < trampolines.Count; ++i){
            Destroy(trampolines[i]);
        } 
        trampolines.Clear();
        for (int i = 0; i < barrotes.Count; ++i){
            Destroy(barrotes[i]);
        } 
        barrotes.Clear();
        for (int i = 0; i < cubes.Count; ++i){
            cubes[i].transform.position = new Vector3 (cubes[i].transform.position.x, cubes[i].transform.position.y - 2.0f * Time.deltaTime, cubes[i].transform.position.z);
        }
        shark1.transform.position = new Vector3 (shark1.transform.position.x, shark1.transform.position.y - 2.0f * Time.deltaTime, shark1.transform.position.z);
        shark2.transform.position = new Vector3 (shark2.transform.position.x, shark2.transform.position.y - 2.0f * Time.deltaTime, shark2.transform.position.z);
        shark3.transform.position = new Vector3 (shark3.transform.position.x, shark3.transform.position.y - 2.0f * Time.deltaTime, shark3.transform.position.z);
        shark4.transform.position = new Vector3 (shark4.transform.position.x, shark4.transform.position.y - 2.0f * Time.deltaTime, shark4.transform.position.z);

        if (cubes[0].transform.position.y <= -6){
            return true;
        }
        return false;
    }

    private void move_sharks_forward(){
        shark1.transform.position = new Vector3 (shark1.transform.position.x, shark1.transform.position.y, shark1.transform.position.z + 0.5f * Time.deltaTime);
        shark2.transform.position = new Vector3 (shark2.transform.position.x, shark2.transform.position.y, shark2.transform.position.z + 0.5f * Time.deltaTime);
        shark3.transform.position = new Vector3 (shark3.transform.position.x, shark3.transform.position.y, shark3.transform.position.z + 0.5f * Time.deltaTime);
        shark4.transform.position = new Vector3 (shark4.transform.position.x, shark4.transform.position.y, shark4.transform.position.z + 0.5f * Time.deltaTime);
    }

    public int player_next_move(Vector3 player_position)
    {
        move_sharks_forward();
        if (player_position.z < 0 && ! finished) return 2;
        if (finished){
            if (keep_destroying){
                if (destroying()){
                    keep_destroying = false;
                    finished = false;
                    Game.Instance.stage_finished();   
                }
            } 
            if (player_position.z < 1){
                if (player_position.z >= -1){
                    return -2;
                }
                keep_destroying = true;
                
            }
            if (player_position.z > 9){
                if (player_position.z <= 9.8){
                    return 2;
                }
                keep_destroying = true;
            }
            if (player_position.x < 1){
                if (player_position.x >= -0.9){
                    return -1;
                }
                keep_destroying = true;
            }
            if (player_position.x > 9){
                if (player_position.x <= 10.1){
                    return 1;
                }
                keep_destroying = true;
            }
            return 0;
        }
        if (Last_pos.x != (int)player_position.x || Last_pos.y != (int)player_position.z){
            ++cube_done;
            Last_pos = new Vector2((int)player_position.x,(int)player_position.z);
        }
        int old_direction = map_positions[(int)player_position.x,(int)player_position.z].old_direction;
        bool moving_to = ((old_direction == -2) && (player_position.z - (float)Math.Floor(player_position.z) <= 0.4)) || ((old_direction == 2) && (player_position.z - (float)Math.Floor(player_position.z) >= 0.05)) || ((old_direction == 1) && (player_position.x - (float)Math.Floor(player_position.x) >= 0.1)) || ((old_direction == -1) && (player_position.x - (float)Math.Floor(player_position.x) <= 0.3));
        if (moving_to) {            
            if (map_positions[(int)player_position.x,(int)player_position.z].end) finished = true;
            return map_positions[(int)player_position.x,(int)player_position.z].next_direction;
        }
        else{
            return  map_positions[(int)player_position.x,(int)player_position.z].old_direction;
        } 
    }

    public int get_percentage(){
        return (int)(100.0f * ((float)current_stage/ (float)n_stages) + (100.0f / (float)n_stages) * ((float)cube_done / (float)n_cubes));
    }    

    public bool has_to_jump(Vector3 position){
        if (position.x <= 10 && position.x >= -1 && position.z <= 10 && position.z >= 0){
            return  map_positions[(int)position.x,(int)position.z].jump;
        }
        return false;       
    }
}