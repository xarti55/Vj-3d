using UnityEngine;
using System.Collections.Generic;


public struct map_position_properties
{
    public float height;
    public int next_direction; //0 = left, 1 = top, 2 = right, 3 = bottom
    public int old_direction; //0 = left, 1 = top, 2 = right, 3 = bottom
} 

public struct lvl_position
{
    public int pos_x;
    public int pos_y;
    public float height;
}


public class Map : MonoBehaviour
{
    private map_position_properties [,] map_positions;
    public static Map Instance; // Singleton Instance
    private lvl_position [,] lv1;
    

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
        map_positions = new map_position_properties [10,10];
        lv1 = new lvl_position[,]{{
            new lvl_position { pos_x = 0, pos_y = 0, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 0, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 1, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 2, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 3, height = 0.0f },
            new lvl_position { pos_x = 2, pos_y = 3, height = 0.0f },
            new lvl_position { pos_x = 3, pos_y = 3, height = 0.0f },
            new lvl_position { pos_x = 4, pos_y = 3, height = 0.0f },
            new lvl_position { pos_x = 4, pos_y = 4, height = 0.0f },
            new lvl_position { pos_x = 4, pos_y = 5, height = 0.0f },
            new lvl_position { pos_x = 3, pos_y = 5, height = 0.0f },
            new lvl_position { pos_x = 2, pos_y = 5, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 5, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 6, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 7, height = 0.0f },
            new lvl_position { pos_x = 1, pos_y = 8, height = 0.0f },
            new lvl_position { pos_x = 2, pos_y = 8, height = 0.0f },
            new lvl_position { pos_x = 3, pos_y = 8, height = 0.0f },
            new lvl_position { pos_x = 5, pos_y = 8, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 8, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 7, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 6, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 5, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 4, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 3, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 2, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 1, height = 0.0f },
            new lvl_position { pos_x = 6, pos_y = 0, height = 0.0f }}};
    }

    public bool player_hit(Vector3 player_position)
    {
       return player_position.z <  map_positions[(int)player_position.x,(int)player_position.y].height - 0.2; 
    }

    public void load_lvl_stage(int level, int lvl_stage){
        if (level == 1){
            for (int i = 0; i <  map_positions[0].Length; ++i){
                return;
            }
        }
    } 

    public int player_next_move(Vector3 player_position)
    {
        int old_direction = map_positions[(int)player_position.x,(int)player_position.y].old_direction;
        bool moving_from = ((old_direction == 0) && (player_position.x - (int)player_position.x <= 0.5)) || ((old_direction == 2) && (player_position.x - (int)player_position.x >= 0.5)) || ((old_direction == 1) && (player_position.y - (int)player_position.y <= 0.5)) || ((old_direction == 4) && (player_position.y - (int)player_position.y >= 0.5));
        if (moving_from) return map_positions[(int)player_position.x,(int)player_position.y].next_direction;
        else return  map_positions[(int)player_position.x,(int)player_position.y].old_direction;
    }
}
