namespace FNFNewBot.Dto
{
    public enum DifficultyMode
    {
        Easy,
        Normal,
        Hard,
        Erect
    }

    public class NoteInfo
    {
        public int Direction { get; set; }
        public double Time { get; set; }
        public double? Length { get; set; }
        public KeyType KeyType { get; set; }

        public static NoteInfo From(Note2 note, KeyType keyType)
        {
            return new NoteInfo
            {
                Direction = note.Direction,
                Time = note.Time,
                Length = note.Length,
                KeyType = keyType
            };
        }

        public static NoteInfo From(double[] noteData, KeyType keyType)
        {
            return new NoteInfo
            {
                Direction = (int)noteData[1],
                Time = noteData[0],
                Length = noteData[2],
                KeyType = keyType
            };
        }
    }

    public class NoteSection
    {
        public List<NoteInfo> Notes { get; set; } = new();
        public DifficultyMode Mode { get; set; }

        public static NoteSection From(List<Note2> notes, DifficultyMode mode, List<KeyType> keyTypes)
        {
            return new NoteSection
            {
                Notes = notes
                    .Where(note => note.Direction < keyTypes.Count)
                    .Select(note => NoteInfo.From(note, keyTypes[note.Direction]))
                    .ToList(),
                Mode = mode
            };
        }

        public static NoteSection From(List<SectionNotes1Detail> notes, DifficultyMode mode, List<KeyType> keyTypes)
        {
            var filteredNotes = notes
                .Where(noteDetail => noteDetail.MustHitSection)
                .SelectMany(noteDetail => noteDetail.SectionNotes)
                .ToList();

            return new NoteSection
            {
                Notes = filteredNotes
                    .Where(noteData => noteData[1] < keyTypes.Count)
                    .Select(noteData => NoteInfo.From(noteData.ToArray(), keyTypes[(int)noteData[1]]))
                    .ToList(),
                Mode = mode
            };
        }
    }

    public class SongInfo
    {
        public int Version { get; set; }
        public string Name { get; set; }
        public List<NoteSection> Sections { get; set; } = new();

        public static SongInfo From(string name, Song2 song, List<KeyType> keyTypes)
        {
            return new SongInfo
            {
                Version = 2,
                Name = name,
                Sections = new List<NoteSection>
                {
                    NoteSection.From(song.Notes.Easy, DifficultyMode.Easy, keyTypes),
                    NoteSection.From(song.Notes.Normal, DifficultyMode.Normal, keyTypes),
                    NoteSection.From(song.Notes.Hard, DifficultyMode.Hard, keyTypes),
                    NoteSection.From(song.Notes.Erect, DifficultyMode.Erect, keyTypes)
                }
            };
        }

        public static SongInfo From(string name, Song1 song, List<KeyType> keyTypes)
        {
            DifficultyMode mode = name.ToLower() switch
            {
                var n when n.Contains("easy") => DifficultyMode.Easy,
                var n when n.Contains("hard") => DifficultyMode.Hard,
                _ => DifficultyMode.Normal
            };

            return new SongInfo
            {
                Version = 1,
                Name = name,
                Sections = new List<NoteSection>
                {
                    NoteSection.From(song.Song.Notes, mode, keyTypes)
                }
            };
        }
    }
}
