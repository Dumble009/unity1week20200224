using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.Linq;

public class RankingManager : MonoBehaviour
{
	NCMBQuery<NCMBObject> query;
	private void Start()
	{
		query = new NCMBQuery<NCMBObject>("Ranking");
		query.OrderByDescending("score");
		query.Limit = RankingPacket.RankingCount;

		query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
			RankingPacket packet = new RankingPacket();
			for (int i = 0; i < RankingPacket.RankingCount; i++)
			{
				packet.Ranks[i] = 0;
				packet.Scores[i] = 0;
			}

			if (e != null)
			{
				ResultManager.Instance.SetRanking(packet);
			}
			else
			{
				int rank = 0;
				foreach (var obj in objList)
				{
					rank++;
					int score = System.Convert.ToInt32(obj["score"]);
					packet.Ranks[rank - 1] = rank;
					packet.Scores[rank - 1] = score;
				}

				ResultManager.Instance.SetRanking(packet);
			}
		});
	}
}
