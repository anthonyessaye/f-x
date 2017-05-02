using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Web.Syndication;

namespace F_X.InformationGathering
{
    class ExtraPageGather
    {
        public ExtraPageGather()
        {

        }

        public async void LoadNews(TextBlock theNewsText, Uri uri)
        {
            try
            {
                SyndicationClient client = new SyndicationClient();
                SyndicationFeed feed = await client.RetrieveFeedAsync(uri);
                if (feed != null)
                {

                    foreach (SyndicationItem item in feed.Items)
                    {
                        if (item.Title.Text.Length > 20) // limit over 20 so we wont get short meaningless titles
                        {
                            theNewsText.Text = item.Title.Text;
                            await Task.Delay(7000);
                        }

                        else
                            continue;

                    }

                }
            }
            catch
            {

            }

            LoadNews(theNewsText, uri); //function to repeat


        }

    }
}
