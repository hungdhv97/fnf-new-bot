using System.Runtime.InteropServices;

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
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public int Direction { get; set; }
        public double Time { get; set; }
        public double? Length { get; set; }
        public string? Special { get; set; }
        public KeyType KeyType { get; set; }
        public uint ScanCode { get; set; }

        public static NoteInfo From(Note2 note, KeyType keyType)
        {
            return new NoteInfo
            {
                Direction = note.Direction,
                Time = note.Time,
                Length = note.Length,
                KeyType = keyType,
                ScanCode = MapVirtualKey(keyType.Code, 0),
            };
        }

        public static NoteInfo From(int Direction, object[] noteData, KeyType keyType)
        {
            return new NoteInfo
            {
                Direction = Direction,
                Time = Convert.ToDouble(noteData[0]),
                Length = Convert.ToDouble(noteData[2]),
                KeyType = keyType,
                ScanCode = MapVirtualKey(keyType.Code, 0),
                Special = noteData.Length > 3 ? Convert.ToString(noteData[3]) : null,
            };
        }

        public static NoteInfo From(Note3 note, KeyType keyType)
        {
            return new NoteInfo
            {
                Direction = note.Id,
                Time = note.Time,
                Length = note.SLen,
                KeyType = keyType,
                ScanCode = MapVirtualKey(keyType.Code, 0),
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
            int keyCount = keyTypes.Count;

            var filteredNotes = notes
                .SelectMany(noteDetail =>
                    noteDetail.MustHitSection
                    ? noteDetail.SectionNotes.Where(noteData => Convert.ToInt32(noteData[1]) < keyCount)
                    : noteDetail.SectionNotes.Where(noteData => Convert.ToInt32(noteData[1]) >= keyCount)
                )
                .OrderBy(noteData => Convert.ToDouble(noteData[0]))
                .ToList();

            return new NoteSection
            {
                Notes = filteredNotes
                    .Select(noteData => NoteInfo.From(Convert.ToInt32(noteData[1]) % keyCount, noteData.ToArray(), keyTypes[Convert.ToInt32(noteData[1]) % keyCount]))
                    .ToList(),
                Mode = mode
            };
        }

        public static NoteSection From(List<Note3> notes, List<KeyType> keyTypes)
        {
            return new NoteSection
            {
                Notes = notes
                    .Where(note => note.Id < keyTypes.Count)
                    .Select(note => NoteInfo.From(note, keyTypes[note.Id]))
                    .ToList(),
                Mode = DifficultyMode.Hard
            };
        }

        public NoteSection ToStart()
        {
            if (Notes.Count > 0)
            {
                double minTime = Notes.Min(note => note.Time);
                foreach (var note in Notes)
                {
                    note.Time -= minTime;
                }
            }
            return this;
        }

        public List<string> GetSpecialNotes()
        {
            return Notes
                .Where(n => !string.IsNullOrEmpty(n.Special))
                .Select(n => n.Special)
                .Distinct()
                .OrderBy(special => special)
                .ToList()!;
        }

        public NoteSection ToRemoveDeadNotes(List<string> deadNotes)
        {
            Notes = Notes
                .Where(n => string.IsNullOrEmpty(n.Special) || !deadNotes.Contains(n.Special))
                .ToList();
            return this;
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

        public static SongInfo From(string name, Song3 song, List<KeyType> keyTypes)
        {
            StrumLine3? firstFilteredStrumLine = song.StrumLines.FirstOrDefault(sl => sl.Type == 1);

            return new SongInfo
            {
                Version = 3,
                Name = name,
                Sections = firstFilteredStrumLine != null
                    ? new List<NoteSection> { NoteSection.From(firstFilteredStrumLine.Notes, keyTypes) }
                    : new List<NoteSection>()
            };
        }
    }
}
