using System;
using System.Collections.Generic;

namespace Example
{
	/// <summary>
	/// Some sample data to show in the example.
	/// </summary>
	/// <remarks>
	/// Data via http://philogb.github.io/jit/static/v20/Jit/Examples/Treemap/example1.code.html
	/// Mostly translated to C# via https://jsontocsharpconverter.web.app/
	/// License: MIT: https://github.com/philogb/jit/blob/571cc88564ac8628b1e93018fd78cfa65ea3b38f/LICENSE
	/// Copyright © 2013 Sencha Inc. - Author: Nicolas Garcia Belmonte (http://philogb.github.com/)
	/// </remarks>
	public static class SampleData
	{
		public class MusicPlaybackData
		{
			public List<MusicPlaybackData> Children { get; set; }
			public PlaybackData Info { get; set; }
			public string Id { get; set; }
			public string Name { get; set; }
		}

		public class PlaybackData
		{
			public long Playcount { get; set; }
			public string Color { get; set; }
			public Uri Image { get; set; }
			public long Area { get; set; }
		}

		public static MusicPlaybackData GetData()
		{
			return new MusicPlaybackData
			{
				Children =
					new List<MusicPlaybackData> {
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 276,
												Color = "#8E7032",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/11403219.jpg"),
												Area = 276
											},
										Id = "album-Thirteenth Step",
										Name = "Thirteenth Step"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 271,
												Color = "#906E32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/11393921.jpg"),
												Area = 271
											},
										Id = "album-Mer De Noms",
										Name = "Mer De Noms"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 547,
									Area = 547
								},
							Id = "artist_A Perfect Circle",
							Name = "A Perfect Circle"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 209,
												Color = "#AA5532",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/32349839.jpg"),
												Area = 209
											},
										Id = "album-Above",
										Name = "Above"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 209,
									Area = 209
								},
							Id = "artist_Mad Season",
							Name = "Mad Season"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 260,
												Color = "#956932",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/38753425.jpg"),
												Area = 260
											},
										Id =
											"album-Tiny Music... Songs From the Vatican Gift Shop",
										Name =
											"Tiny Music... Songs From the Vatican Gift Shop"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 254,
												Color = "#976732",
												Image =
													new Uri("http://images.amazon.com/images/P/B000002IU3.01.LZZZZZZZ.jpg"),
												Area = 254
											},
										Id = "album-Core",
										Name = "Core"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 514,
									Area = 514
								},
							Id = "artist_Stone Temple Pilots",
							Name = "Stone Temple Pilots"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 181,
												Color = "#B54932",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8673371.jpg"),
												Area = 181
											},
										Id = "album-The Science of Things",
										Name = "The Science of Things"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 181,
									Area = 181
								},
							Id = "artist_Bush",
							Name = "Bush"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 229,
												Color = "#A15D32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/32579429.jpg"),
												Area = 229
											},
										Id =
											"album-Echoes, Silence, Patience &amp; Grace",
										Name =
											"Echoes, Silence, Patience &amp; Grace"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 185,
												Color = "#B34B32",
												Image =
													new Uri("http://images.amazon.com/images/P/B0009HLDFU.01.MZZZZZZZ.jpg"),
												Area = 185
											},
										Id = "album-In Your Honor (disc 2)",
										Name = "In Your Honor (disc 2)"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 414,
									Area = 414
								},
							Id = "artist_Foo Fighters",
							Name = "Foo Fighters"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 398,
												Color = "#5DA132",
												Image =
													new Uri("http://images.amazon.com/images/P/B00005LNP5.01._SCMZZZZZZZ_.jpg"),
												Area = 398
											},
										Id = "album-Elija Y Gane",
										Name = "Elija Y Gane"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 203,
												Color = "#AC5232",
												Image =
													new Uri("http://images.amazon.com/images/P/B0000B193V.01._SCMZZZZZZZ_.jpg"),
												Area = 203
											},
										Id = "album-Para los Arboles",
										Name = "Para los Arboles"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 601,
									Area = 601
								},
							Id = "artist_Luis Alberto Spinetta",
							Name = "Luis Alberto Spinetta"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 224,
												Color = "#A35B32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/26497553.jpg"),
												Area = 224
											},
										Id = "album-Music Bank",
										Name = "Music Bank"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 217,
												Color = "#A65832",
												Image =
													new Uri("http://images.amazon.com/images/P/B0000296JW.01.MZZZZZZZ.jpg"),
												Area = 217
											},
										Id = "album-Music Bank (disc 1)",
										Name = "Music Bank (disc 1)"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 215,
												Color = "#A75732",
												Image =
													new Uri("http://images.amazon.com/images/P/B0000296JW.01.MZZZZZZZ.jpg"),
												Area = 215
											},
										Id = "album-Music Bank (disc 2)",
										Name = "Music Bank (disc 2)"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 181,
												Color = "#B54932",
												Image =
													new Uri("http://images.amazon.com/images/P/B0000296JW.01.MZZZZZZZ.jpg"),
												Area = 181
											},
										Id = "album-Music Bank (disc 3)",
										Name = "Music Bank (disc 3)"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 837,
									Area = 837
								},
							Id = "artist_Alice in Chains",
							Name = "Alice in Chains"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 627,
												Color = "#00FF32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8480501.jpg"),
												Area = 627
											},
										Id = "album-10,000 Days",
										Name = "10,000 Days"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 627,
									Area = 627
								},
							Id = "artist_Tool",
							Name = "Tool"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 261,
												Color = "#946A32",
												Image =
													new Uri("http://cdn.last.fm/flatness/catalogue/noimage/2/default_album_medium.png"),
												Area = 261
											},
										Id =
											"album-2006-09-07: O-Bar, Stockholm, Sweden",
										Name =
											"2006-09-07: O-Bar, Stockholm, Sweden"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 211,
												Color = "#A95532",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/25402479.jpg"),
												Area = 211
											},
										Id = "album-Lost and Found",
										Name = "Lost and Found"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 472,
									Area = 472
								},
							Id = "artist_Chris Cornell",
							Name = "Chris Cornell"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 197,
												Color = "#AE5032",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8634627.jpg"),
												Area = 197
											},
										Id = "album-The Sickness",
										Name = "The Sickness"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 197,
									Area = 197
								},
							Id = "artist_Disturbed",
							Name = "Disturbed"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 493,
												Color = "#36C832",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8591345.jpg"),
												Area = 493
											},
										Id = "album-Mama's Gun",
										Name = "Mama's Gun"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 493,
									Area = 493
								},
							Id = "artist_Erykah Badu",
							Name = "Erykah Badu"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 249,
												Color = "#996532",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/32070871.jpg"),
												Area = 249
											},
										Id = "album-Audioslave",
										Name = "Audioslave"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 249,
									Area = 249
								},
							Id = "artist_Audioslave",
							Name = "Audioslave"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 359,
												Color = "#6C9232",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/15858421.jpg"),
												Area = 359
											},
										Id =
											"album-Comfort y Música Para Volar",
										Name = "Comfort y Música Para Volar"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 359,
									Area = 359
								},
							Id = "artist_Soda Stereo",
							Name = "Soda Stereo"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 302,
												Color = "#847A32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8776205.jpg"),
												Area = 302
											},
										Id = "album-Clearing the Channel",
										Name = "Clearing the Channel"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 302,
									Area = 302
								},
							Id = "artist_Sinch",
							Name = "Sinch"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 177,
												Color = "#B74732",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/32457599.jpg"),
												Area = 177
											},
										Id = "album-Crash",
										Name = "Crash"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 177,
									Area = 177
								},
							Id = "artist_Dave Matthews Band",
							Name = "Dave Matthews Band"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 207,
												Color = "#AA5432",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/30352203.jpg"),
												Area = 207
											},
										Id = "album-Vs.",
										Name = "Vs."
									}
								},
							Info =
								new PlaybackData {
									Playcount = 207,
									Area = 207
								},
							Id = "artist_Pearl Jam",
							Name = "Pearl Jam"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 486,
												Color = "#39C532",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/26053425.jpg"),
												Area = 486
											},
										Id = "album-It All Makes Sense Now",
										Name = "It All Makes Sense Now"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 251,
												Color = "#986632",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/9658733.jpg"),
												Area = 251
											},
										Id = "album-Air",
										Name = "Air"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 737,
									Area = 737
								},
							Id = "artist_Krøm",
							Name = "Krøm"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 345,
												Color = "#728C32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8605651.jpg"),
												Area = 345
											},
										Id = "album-Temple Of The Dog",
										Name = "Temple Of The Dog"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 345,
									Area = 345
								},
							Id = "artist_Temple of the Dog",
							Name = "Temple of the Dog"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 318,
												Color = "#7D8132",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/29274729.jpg"),
												Area = 318
											},
										Id =
											"album-And All That Could Have Been (Still)",
										Name =
											"And All That Could Have Been (Still)"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 318,
									Area = 318
								},
							Id = "artist_Nine Inch Nails",
							Name = "Nine Inch Nails"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 256,
												Color = "#966832",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/32595059.jpg"),
												Area = 256
											},
										Id = "album-Mamagubida",
										Name = "Mamagubida"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 220,
												Color = "#A55932",
												Image =
													new Uri("http://cdn.last.fm/flatness/catalogue/noimage/2/default_album_medium.png"),
												Area = 220
											},
										Id = "album-Reggae à Coup de Cirque",
										Name = "Reggae à Coup de Cirque"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 181,
												Color = "#B54932",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/16799743.jpg"),
												Area = 181
											},
										Id = "album-Grain de sable",
										Name = "Grain de sable"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 657,
									Area = 657
								},
							Id = "artist_Tryo",
							Name = "Tryo"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 258,
												Color = "#966832",
												Image =
													new Uri("http://cdn.last.fm/flatness/catalogue/noimage/2/default_album_medium.png"),
												Area = 258
											},
										Id = "album-Best Of",
										Name = "Best Of"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 176,
												Color = "#B74732",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/5264426.jpg"),
												Area = 176
											},
										Id = "album-Robbin' The Hood",
										Name = "Robbin' The Hood"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 434,
									Area = 434
								},
							Id = "artist_Sublime",
							Name = "Sublime"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 418,
												Color = "#55AA32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8590493.jpg"),
												Area = 418
											},
										Id = "album-One Hot Minute",
										Name = "One Hot Minute"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 418,
									Area = 418
								},
							Id = "artist_Red Hot Chili Peppers",
							Name = "Red Hot Chili Peppers"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 275,
												Color = "#8F6F32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/17597653.jpg"),
												Area = 275
											},
										Id = "album-Chinese Democracy",
										Name = "Chinese Democracy"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 203,
												Color = "#AC5232",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/15231979.jpg"),
												Area = 203
											},
										Id = "album-Use Your Illusion II",
										Name = "Use Your Illusion II"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 478,
									Area = 478
								},
							Id = "artist_Guns N' Roses",
							Name = "Guns N' Roses"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 208,
												Color = "#AA5432",
												Image =
													new Uri("http://images.amazon.com/images/P/B0007LCNNE.01.MZZZZZZZ.jpg"),
												Area = 208
											},
										Id =
											"album-Tales of the Forgotten Melodies",
										Name = "Tales of the Forgotten Melodies"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 208,
									Area = 208
								},
							Id = "artist_Wax Tailor",
							Name = "Wax Tailor"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 208,
												Color = "#AA5432",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/7862623.png"),
												Area = 208
											},
										Id = "album-In Rainbows",
										Name = "In Rainbows"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 208,
									Area = 208
								},
							Id = "artist_Radiohead",
							Name = "Radiohead"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 317,
												Color = "#7E8032",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8600371.jpg"),
												Area = 317
											},
										Id = "album-Down On The Upside",
										Name = "Down On The Upside"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 290,
												Color = "#897532",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8590515.jpg"),
												Area = 290
											},
										Id = "album-Superunknown",
										Name = "Superunknown"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 607,
									Area = 607
								},
							Id = "artist_Soundgarden",
							Name = "Soundgarden"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 247,
												Color = "#9A6432",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/15113951.jpg"),
												Area = 247
											},
										Id = "album-Nico",
										Name = "Nico"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 218,
												Color = "#A65832",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/45729417.jpg"),
												Area = 218
											},
										Id = "album-Soup",
										Name = "Soup"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 197,
												Color = "#AE5032",
												Image =
													new Uri("http://images.amazon.com/images/P/B00005V5PW.01.MZZZZZZZ.jpg"),
												Area = 197
											},
										Id = "album-Classic Masters",
										Name = "Classic Masters"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 194,
												Color = "#B04E32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/15157989.jpg"),
												Area = 194
											},
										Id = "album-Blind Melon",
										Name = "Blind Melon"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 856,
									Area = 856
								},
							Id = "artist_Blind Melon",
							Name = "Blind Melon"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 537,
												Color = "#24DA32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/17594883.jpg"),
												Area = 537
											},
										Id = "album-Make Yourself",
										Name = "Make Yourself"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 258,
												Color = "#966832",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/31550385.jpg"),
												Area = 258
											},
										Id = "album-Light Grenades",
										Name = "Light Grenades"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 181,
												Color = "#B54932",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/32309285.jpg"),
												Area = 181
											},
										Id = "album-Morning View",
										Name = "Morning View"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 976,
									Area = 976
								},
							Id = "artist_Incubus",
							Name = "Incubus"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 198,
												Color = "#AE5032",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/8599099.jpg"),
												Area = 198
											},
										Id = "album-On And On",
										Name = "On And On"
									},
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 186,
												Color = "#B34B32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/30082075.jpg"),
												Area = 186
											},
										Id = "album-Brushfire Fairytales",
										Name = "Brushfire Fairytales"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 384,
									Area = 384
								},
							Id = "artist_Jack Johnson",
							Name = "Jack Johnson"
						},
						new MusicPlaybackData {
							Children =
								new List<MusicPlaybackData> {
									new MusicPlaybackData {
										Children =
											new List<MusicPlaybackData> { },
										Info =
											new PlaybackData {
												Playcount = 349,
												Color = "#718D32",
												Image =
													new Uri("http://userserve-ak.last.fm/serve/300x300/21881921.jpg"),
												Area = 349
											},
										Id = "album-Mother Love Bone",
										Name = "Mother Love Bone"
									}
								},
							Info =
								new PlaybackData {
									Playcount = 349,
									Area = 349
								},
							Id = "artist_Mother Love Bone",
							Name = "Mother Love Bone"
						}
					},
				Info = new PlaybackData { },
				Id = "root",
				Name = "Top Albums"
			};
		}
	}
}
