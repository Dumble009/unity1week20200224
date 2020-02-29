using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.Linq;

public class RankingManager : SingletonMonoBehaviour<RankingManager>
{
	NCMBQuery<NCMBObject> query;
	public RankingPacket Packet { get; set; }
	private void Start()
	{
		query = new NCMBQuery<NCMBObject>("Ranking");
		query.OrderByDescending("score");
		query.Limit = RankingPacket.RankingCount;

		query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
			Packet = new RankingPacket();
			for (int i = 0; i < RankingPacket.RankingCount; i++)
			{
				Packet.Ranks[i] = 0;
				Packet.Scores[i] = 0;
			}

			if (e != null)
			{
				ResultManager.Instance.SetRanking(Packet);
			}
			else
			{
				int rank = 0;
				foreach (var obj in objList)
				{
					rank++;
					int score = System.Convert.ToInt32(obj["score"]);
					Packet.Ranks[rank - 1] = rank;
					Packet.Scores[rank - 1] = score;
				}

				//ResultManager.Instance.SetRanking(Packet);
			}
		});
	}

	public void SetScore(int score)
	{
		NCMBObject obj = new NCMBObject("Ranking");
		obj["score"] = score;

		obj.SaveAsync();
	}
}
