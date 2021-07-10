using System;
using System.Collections.Generic;
using System.Text;


public interface ITurnLimitedEffect 
{
    public int SetStatusTurns();
}
public interface ICauseNVStatus
{
    public NonVolatileStatus StatusCaused();
}
public interface ICauseVStatus
{
    public VolatileStatus StatusCaused();
}
public interface INonTrainableMove
{

}
