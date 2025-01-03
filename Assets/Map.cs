using UnityEngine;
using System.Collections.Generic;
using System;

//aa
public struct map_position_properties
{
    public float height;
    public int next_direction; //-1 = left, -2 = bottom, 1 = right, 2 = top
    public int old_direction; //-1 = left, -2 = bottom, 1 = right, 2 = top
} 

public struct lvl_position
{
    public int pos_x;
    public int pos_z;
    public float height;
}


public class Map : MonoBehaviour
{
    private map_position_properties [,] map_positions;
    public static Map Instance; // Singleton Instance
    private lvl_position [,] lv1;
    private List <GameObject> cubes;
    int current_stage, n_stages, cube_done, n_cubes;
    int from_to = 0; // 0 = from, 1 = to
    

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
        cubes = new List<GameObject>();
        map_positions = new map_position_properties [10,10];
        lv1 = new lvl_position[,]{{
            new lvl_position { pos_x = 0, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 0, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 4, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 2, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 5, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 6, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 1, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 2, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 3, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 4, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 5, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 8, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 7, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 6, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 5, height = 2.0f },
            new lvl_position { pos_x = 6, pos_z = 4, height = 3.0f },
            new lvl_position { pos_x = 6, pos_z = 3, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 2, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 1, height = 1.0f },
            new lvl_position { pos_x = 6, pos_z = 0, height = 1.0f }}};
    }
    public bool player_hit(Vector3 player_position)
    {
       return player_position.z <  (map_positions[(int)player_position.x,(int)player_position.y].height - 0.2); 
    }

    private void add_cube_to_position(lvl_position pos){
        
        for (int i = 0; i < pos.height; ++i){
            cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
            cubes[cubes.Count - 1].transform.position = new Vector3(pos.pos_x,i, pos.pos_z);
        }
        
    }

    public void load_lvl_stage(int level, int lvl_stage){        
         if (level == 1){
            current_stage = lvl_stage;
            n_stages = lv1.GetLength(0);
            cube_done = 0;
            n_cubes = 0;
            add_cube_to_position(lv1[lvl_stage, 0]);
            int last_pos = lv1.GetLength(lvl_stage + 1) - 1;
            map_positions[lvl_stage,0].height = lv1[lvl_stage,0].height;
            map_positions[lvl_stage,0].old_direction = 2;
            map_positions[lvl_stage,0].next_direction = (lv1[lvl_stage,0].pos_x != lv1[lvl_stage,1].pos_x) ?  lv1[lvl_stage,1].pos_x - lv1[lvl_stage,0].pos_x   : (lv1[lvl_stage,1].pos_z - lv1[lvl_stage,0].pos_z)*2;
            ++n_cubes;
            for (int i = 1; i <  lv1.GetLength(lvl_stage + 1) - 1; ++i){
                add_cube_to_position(lv1[lvl_stage,i]);
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].old_direction = (lv1[lvl_stage,i].pos_x != lv1[lvl_stage,i - 1].pos_x) ?  lv1[lvl_stage,i].pos_x - lv1[lvl_stage,i - 1].pos_x   : (lv1[lvl_stage,i].pos_z - lv1[lvl_stage,i - 1].pos_z)*2;
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].next_direction = (lv1[lvl_stage,i].pos_x != lv1[lvl_stage,i + 1].pos_x) ?  lv1[lvl_stage,i + 1].pos_x - lv1[lvl_stage,i].pos_x   : (lv1[lvl_stage,i + 1].pos_z - lv1[lvl_stage,i].pos_z)*2;
                map_positions[lv1[lvl_stage,i].pos_x,lv1[lvl_stage,i].pos_z].height = lv1[lvl_stage,i].height;
                ++n_cubes;
            }
            add_cube_to_position(lv1[lvl_stage,last_pos]);
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].old_direction = (lv1[lvl_stage,last_pos].pos_x != lv1[lvl_stage,last_pos - 1].pos_x) ? lv1[lvl_stage,last_pos].pos_x - lv1[lvl_stage,last_pos - 1].pos_x   : (lv1[lvl_stage,last_pos].pos_z - lv1[lvl_stage,last_pos - 1].pos_z)*2;
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].next_direction =  (lv1[lvl_stage,last_pos].pos_x != lv1[lvl_stage,last_pos - 1].pos_x) ? lv1[lvl_stage,last_pos].pos_x - lv1[lvl_stage,last_pos - 1].pos_x   : (lv1[lvl_stage,last_pos].pos_z - lv1[lvl_stage,last_pos - 1].pos_z)*2;
            map_positions[lv1[lvl_stage,last_pos].pos_x,lv1[lvl_stage,last_pos].pos_z].height = lv1[lvl_stage,last_pos].height;
            ++n_cubes;
         }
    } 

    public int player_next_move(Vector3 player_position)
    {
        if (player_position.x > 10 || player_position.x < 0 || player_position.z > 10 || player_position.z < 0 ) return 0; //We've finished the stage
        int old_direction = map_positions[(int)player_position.x,(int)player_position.z].old_direction;
        bool moving_from = ((old_direction == -2) && (player_position.z - (float)Math.Floor(player_position.z) >= 0.5)) || ((old_direction == 2) && (player_position.z - (float)Math.Floor(player_position.z) <= 0.5)) || ((old_direction == 1) && (player_position.x - (float)Math.Floor(player_position.x) <= 0.5)) || ((old_direction == -1) && (player_position.x - (float)Math.Floor(player_position.x) >= 0.5));
        if (moving_from) {
            from_to = 0;
            return map_positions[(int)player_position.x,(int)player_position.z].next_direction;
        }
        else{
            if (from_to == 0)  ++cube_done;          
            from_to = 1;
            return  map_positions[(int)player_position.x,(int)player_position.z].old_direction;
        } 
    }

    public int get_percentage(){
        return (int)(100.0f * ((float)current_stage / (float)n_stages) + (100.0f / (float)n_stages) * ((float)cube_done / (float)n_cubes));
    }

    
}
