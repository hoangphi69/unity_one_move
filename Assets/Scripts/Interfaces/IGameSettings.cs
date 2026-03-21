public interface IGameSettings
{
  void InitDefaults();
  void Load();
  void Save();
  void Apply();
}