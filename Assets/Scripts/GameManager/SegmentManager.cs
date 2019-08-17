using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using LevelInfo;

public class SegmentManager : MonoBehaviour
{  
    
    private int next_complexity;
    private static Vector2 next_position;

    private bool spawn_station;

    [SerializeField] private Tracks tracks_object;
    [SerializeField] private GameHandler game_handler;
    [SerializeField] private TrainController train_controller_object;
    
    private Dictionary<int,Level> generated_levels = new Dictionary<int,Level>();
    Segment st1;
    void Start()
    {
        Global.Instance.segment_manager = this;
        next_position = Vector2.up*-2;
    }


    // TO-DO: Generate Level based on current level
    private void GenerateLevel(int level_number, Style style){

        Level new_level;
        switch(style){
            case Style.Standart:
                new_level = new Standart();
                break;
            case Style.SimpleShort:
                new_level = new SimpleShort();
                break;
            case Style.SimpleLong:
                new_level = new SimpleLong();
                break;
            case Style.ComplexShort:
                new_level = new ComplexShort();
                break;
            case Style.ComplexLong:
                new_level = new ComplexLong();
                break;
            case Style.GoldRich:
                new_level = new GoldRich();
                break;
            default:
                new_level = new Standart();
                break;
        }
        
        generated_levels.Add(level_number,new_level);


        Segment station= SpawnSegment("station");
        new_level.start_station = station;
        new_level.level_state = LevelState.Queued;
        new_level.number = level_number;
        

        new_level.GenerateTracks();
        
        if(generated_levels.ContainsKey(level_number-1)){
            Level prev_level =  generated_levels[level_number - 1];
            prev_level.end_station = station;
        }
        
        //GenerateTrees(new_level.start_station,new_level.end_station);
        //GenerateCows(new_level.start_station,new_level.end_station);
        
        tracks_object.UpdateArrays();
        //GameHandler.UpdateNextJunction();
        game_handler.AddLevel(new_level);
    }

    
    public void UpdateMap(List<int> needs_to_exist){
        needs_to_exist.Sort();
        foreach (int num in needs_to_exist)
        {
            if(!generated_levels.ContainsKey(num)){
                GenerateLevel(num,Level.CalculateStyle(num));
            }
        }
    }

    
    [System.Serializable]
    public struct Palette
    {
        public float z_position;

        public Color floor_color;
        public Color bar_color;
        public Color tree_trunk;
        public Color leaves;

    }
    // Update is called once per frame
    [Header("Color Settings")]
    public Color train_color;
    public List<Palette> palettes;
    


    [Header("Tree Settings")]
    [Range(0,1)] public float tree_min = 0.8f;
    [Range(0,1)] public float tree_max = 1;
    private void GenerateTrees(Segment start_station, Segment end_station){
        float min_z = start_station.transform.position.z + start_station.get_start_offset().y - 5f;
        float max_z = end_station.transform.position.z + end_station.get_end_offset().y + 5f;

        float min_x = Mathf.Min(start_station.transform.position.x,end_station.transform.position.x)-5f;
        float max_x = Mathf.Max(start_station.transform.position.x,end_station.transform.position.x)+5f;

        float res = 2f;

        for (float z = min_z; z <= max_z; z = z + res)
        {
            for (float x = min_x; x <= max_x; x = x + res)
            {
                float result = Mathf.PerlinNoise(x/10,z/10);

                if(result >= tree_min && result <= tree_max){
                    SpawnTree(x,z);
                }
            }
        }
    }

    [Header("Cow settings")]
    [Range(0,1)] public float cow_min = 0.8f;
    [Range(0,1)] public float cow_max = 1;
    private void GenerateCows(Segment start_station, Segment end_station){
        float min_z = start_station.transform.position.z + start_station.get_start_offset().y - 5f;
        float max_z = end_station.transform.position.z + end_station.get_end_offset().y + 5f;

        float min_x = Mathf.Min(start_station.transform.position.x,end_station.transform.position.x)-5f;
        float max_x = Mathf.Max(start_station.transform.position.x,end_station.transform.position.x)+5f;

        float res = 5f;

        for (float z = min_z; z <= max_z; z = z + res)
        {
            for (float x = min_x; x <= max_x; x = x + res)
            {
                float result = Mathf.PerlinNoise(x/10,z/10);

                if(result >= tree_min && result <= tree_max){
                    SpawnCow(x,z);
                }
            }
        }
    }

    private void SpawnTree(float x, float y){
        GameObject obj = ObjectPool.Instance.SpawnFromPool("tree");
        obj.transform.localScale = new Vector3(0.3f,0.3f + rand.Next(-10,11)/200f,0.3f);
        obj.transform.position = new Vector3(x + rand.Next(-10,11)/15f,rand.Next(-30,0)/300f,y + rand.Next(-10,11)/15f);
        obj.GetComponent<TreeModel>().Reset();

        obj.GetComponent<TreeModel>().SetTrunkColor(CalculateTrunkColor(obj.transform.position.z));
        obj.GetComponent<TreeModel>().SetLeavesColor(CalculateLeavesColor(obj.transform.position.z));
    }

    private void SpawnCow(float x, float y){
        GameObject obj = ObjectPool.Instance.SpawnFromPool("cow");
        obj.transform.position = new Vector3(x + rand.Next(-10,11)/25f,0,y + rand.Next(-10,11)/25f);
        obj.transform.eulerAngles = new Vector3(0,rand.Next(-80,90),0);
    }

    public Segment SpawnSegment(string tag){
        Segment new_segment = ObjectPool.Instance.SpawnFromPool(tag).GetComponent<Segment>();
        
        print(tag);
        new_segment.gameObject.transform.SetParent(tracks_object.transform);
        Vector2 start_offset = new_segment.get_start_offset();
        Vector2 end_offset = new_segment.get_end_offset();
        new_segment.transform.position = ToWorld(next_position - start_offset);
        next_position = (next_position + (end_offset - start_offset));
        if(tag == "station"){
            Global.Instance.station_queue.Enqueue(new_segment);
            EventManager.EmitEvent("new_station");
        }
        new_segment.ResetSegment();
        return new_segment;
    }


    private static System.Random rand = new System.Random();

    public static System.Random random{
        get{
            return rand;
        }
    }

    
    public static string GetTagWithComplexity(int complexity){
        List<string> list = ObjectPool.Instance.getTags(complexity);
        int min = 0;
        int max = list.Count;
        int index = rand.Next(min,max);
        return list[index];
    } 

    /*
    private string CalculateNextTag(){
        if(spawn_station){
            spawn_station = false;
            return "station";
        }
        string tag = GetTagWithComplexity(next_complexity);
        if(rand.Next(0,3 + next_complexity) == 2){
            next_complexity = 2;
        }else{
            next_complexity = 1;
        }
        return tag;
    } */

    /*
    private Segment SpawnNextSegment(){
        string tag = CalculateNextTag();
        Segment seg = SpawnSegment(tag);
        return seg;
    } */

    private static Vector3 ToWorld(Vector2 vec){
        return new Vector3(vec.x*1.7f,0.2f,vec.y*5);
    }

    private static Vector2 ToGrid(Vector3 vec){
        return new Vector2(vec.x/1.7f,vec.z/5);
    }

   
    [Header("References")]
    public GameObject floor;


    private float train_z;
    void Update()
    {   
        if(game_handler.get_train_head == null){
            return;
        }
        train_z = game_handler.get_train_head.transform.position.z;

        UpdateFloorColor(train_z);
        UpdateBarColor(train_z);
    }

    private void UpdateFloorColor(float z){
        Palette palette1 = palettes[0];
        Palette palette2 = palettes[1];

        if(z < palette1.z_position){
            floor.GetComponent<MeshRenderer>().material.color = palette1.floor_color;
            return;
        }

        if(z > palette2.z_position){
             floor.GetComponent<MeshRenderer>().material.color = palette2.floor_color;
             return;
        }
        Color new_color = (((z-palette1.z_position)*palette2.floor_color) + ((palette2.z_position-z)*palette1.floor_color))/(palette2.z_position-palette1.z_position);
        floor.GetComponent<MeshRenderer>().material.SetColor("_Color",new_color);
    }

    private void UpdateBarColor(float z){
        Palette palette1 = palettes[0];
        Palette palette2 = palettes[1];

        if(z < palette1.z_position){
            LevelBar.Instance.UpdateFill_Color(palette1.bar_color);
            return;
        }

        if(z > palette2.z_position){
            LevelBar.Instance.UpdateFill_Color(palette2.bar_color);
             return;
        }
        Color new_color = (((z-palette1.z_position)*palette2.bar_color) + ((palette2.z_position-z)*palette1.bar_color))/(palette2.z_position-palette1.z_position);
        LevelBar.Instance.UpdateFill_Color(new_color);
    }

    private Color CalculateTrunkColor(float tree_z){
        Palette palette1 = palettes[0];
        Palette palette2 = palettes[1];
        Color new_color;

        if(tree_z < palette1.z_position){
            new_color =  palette1.tree_trunk;
        }

        if(tree_z > palette2.z_position){
            new_color = palette2.tree_trunk;
        }
        new_color = (((tree_z-palette1.z_position)*palette2.tree_trunk) + ((palette2.z_position-tree_z)*palette1.tree_trunk))/(palette2.z_position-palette1.z_position);
        
        return new_color + new Color(rand.Next(-50,51)/1500f,rand.Next(-50,51)/1500f,rand.Next(-50,51)/1500f);
    }

    private Color CalculateLeavesColor(float tree_z){
        Palette palette1 = palettes[0];
        Palette palette2 = palettes[1];
        Color new_color;

        if(tree_z < palette1.z_position){
            new_color =  palette1.leaves;
        }

        if(tree_z > palette2.z_position){
            new_color = palette2.leaves;
        }
        new_color = (((tree_z-palette1.z_position)*palette2.leaves) + ((palette2.z_position-tree_z)*palette1.leaves))/(palette2.z_position-palette1.z_position);
        
        return new_color + new Color(rand.Next(-50,51)/500f,rand.Next(-50,51)/500f,rand.Next(-50,51)/500f);
    }
}
