public class characterAbase{
	private int baseValue, buffValue, expToLvl;
	private float lvlMod;
	
	public characterAbase(){
		baseValue = 0;
		buffValue = 0;
		lvlMod = 1.1f;
		expToLvl = 100;
	}
	
	#region Basic Setters and Getters
	public int BaseValue{ 
		get{return baseValue;} 
		set{baseValue = value;}
	}
	
	public int BuffValue{ 
		get{return buffValue;} 
		set{buffValue = value;}
	}
	
	public int ExpToLvl{ 
		get{return expToLvl;} 
		set{expToLvl = value;}
	}
	
	public float LvlMod{ 
		get{return lvlMod;} 
		set{lvlMod = value;}
	}
	#endregion
	
	private int FindExpToLvl(){
		return (int)(expToLvl*lvlMod);	
	}
	
	public void LevelUp(){
		expToLvl = FindExpToLvl();
		baseValue += 1;
	}
	
	public int AdjValue(){
		return baseValue+buffValue;	
	}
}
