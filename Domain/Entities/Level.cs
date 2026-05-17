using Domain.Interfaces;
using Domain.Enums;

namespace Domain.Entities;

public class Level : IEntity
{
    public int Id { get; private set; }
    public CefrLevel CefrCode { get; private set; }
    public string Name { get; private set; }

    private Level(CefrLevel cefrCode, string name)
    {
        CefrCode = cefrCode;
        Name = name;
    }

    public static Level New(CefrLevel cefrCode, string name)
        => new Level(cefrCode, name);
}