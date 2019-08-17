using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelInfo{

    public enum LevelState{
        Queued,
        Current,
        Finished,
        Failed
    }

    public enum Style{
        Standart,
        GoldRich, //Bonus
        ComplexLong,
        ComplexShort,
        SimpleLong,
        SimpleShort
    }
    
    public abstract class Level {
        public int number;
        public Segment start_station;
        public Segment end_station;

        public List<Segment> segments_in_level;

        public LevelState level_state;
        
        public Style style;

        public abstract void GenerateTracks();

        public static Style CalculateStyle(int number){
            if(number%5 == 0 && number != 0){
                return Style.GoldRich;
            }
            if(number == 0){
                return Style.Standart;
            }

            if(number%6 == 0){
                return Style.SimpleLong;
            }

            if(number%8 == 0){
                return Style.ComplexShort;
            }
            if(number%11 == 0){
                return Style.ComplexLong;
            }
            if(number%3 == 0){
                return Style.SimpleShort;
            }

            return Style.Standart;
        }

    }

    public class Standart : Level{
        
        public Standart(){
            style = Style.Standart;
        }

        override public void GenerateTracks(){
            int count = 3 + SegmentManager.random.Next(-1,2);
            int complexity = 1;

            for (int i = 0; i < count; i++)
            {
                string tag = SegmentManager.GetTagWithComplexity(complexity);
                Global.Instance.segment_manager.SpawnSegment(tag);
                complexity = complexity == 1 ? 2 : 1;
            }
        }
    }

    public class ComplexLong : Level{
        
        public ComplexLong(){
            style = Style.ComplexLong;
        }

        override public void GenerateTracks(){
            int count = 4 + SegmentManager.random.Next(-1,2);
            int complexity = 2;

            for (int i = 0; i < count; i++)
            {
                string tag = SegmentManager.GetTagWithComplexity(complexity);
                Global.Instance.segment_manager.SpawnSegment(tag);
            }
        }
    }

    public class ComplexShort : Level{
        
        public ComplexShort(){
            style = Style.ComplexShort;
        }

        override public void GenerateTracks(){
            int count = 2 + SegmentManager.random.Next(-1,2);
            int complexity = 2;

            for (int i = 0; i < count; i++)
            {
                string tag = SegmentManager.GetTagWithComplexity(complexity);
                Global.Instance.segment_manager.SpawnSegment(tag);
            }
        }
    }

    public class SimpleShort : Level{
        
        public SimpleShort(){
            style = Style.SimpleShort;
        }

        override public void GenerateTracks(){
            int count = 2 + SegmentManager.random.Next(-1,2);
            int complexity = 1;

            for (int i = 0; i < count; i++)
            {
                string tag = SegmentManager.GetTagWithComplexity(complexity);
                Global.Instance.segment_manager.SpawnSegment(tag);
            }
        }
    }

    public class SimpleLong : Level{
        
        public SimpleLong(){
            style = Style.SimpleLong;
        }

        override public void GenerateTracks(){
            int count = 5 + SegmentManager.random.Next(-1,2);
            int complexity = 1;

            for (int i = 0; i < count; i++)
            {
                string tag = SegmentManager.GetTagWithComplexity(complexity);
                Global.Instance.segment_manager.SpawnSegment(tag);
            }
        }
    }

    public class GoldRich : Level{
        
        public GoldRich(){
            style = Style.GoldRich;
        }

        override public void GenerateTracks(){
            int count = 3 + Mathf.Clamp((this.number/10),0,3);
            int complexity = 1;

            for (int i = 0; i < count; i++)
            {
                string tag = SegmentManager.GetTagWithComplexity(complexity);
                Segment seg = Global.Instance.segment_manager.SpawnSegment(tag);
                seg.gold_rich = true;
                seg.ResetSegment();
            }
        }
    }
}
