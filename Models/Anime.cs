namespace AnimeExporter {
    public class Anime {
        public string Title { get; internal set; }
        public string Url { get; internal set; }
        // rating
        // categories
        // rank
        // popularity
        // members
        // synopsis
        // background
        // adaptation
        // summary
        // sequel
        // related
        // rating
        // type
        // number of episodes
        // status
        // aired
        // permiered
        // broadcast
        // producers
        // licensors
        // studios
        // source
        // duration
        // link to image
        
        public override string ToString() {
            return
                $"title: {Title} " +
                $"url: {Url} ";
        }
    }
}