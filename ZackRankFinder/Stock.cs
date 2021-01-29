using System;
using System.Collections.Generic;
using System.Text;

namespace ZackRankFinder
{
    public class Sungard
    {
        public string bidasksize { get; set; }
        public string dividend_freq { get; set; }
        public string prev_close_date { get; set; }
        public string timestamp { get; set; }
        public string exchange { get; set; }
        public string shares { get; set; }
        public string volatility { get; set; }
        public string zacks_recommendation { get; set; }
        public string pos_size { get; set; }
        public string open { get; set; }
        public string yrlow { get; set; }
        public string type { get; set; }
        public string yield { get; set; }
        public string market_cap { get; set; }
        public string ask { get; set; }
        public string dividend { get; set; }
        public string dividend_date { get; set; }
        public string earnings { get; set; }
        public string close { get; set; }
        public string day_low { get; set; }
        public string last_trade_datetime { get; set; }
        public string volume { get; set; }
        public string yrhigh { get; set; }
        public string day_high { get; set; }
        public string bid { get; set; }
        public string name { get; set; }
        public string pe_ratio { get; set; }
        public string updated { get; set; }
    }

    public class Bats
    {
        public string ask_size { get; set; }
        public string routed { get; set; }
        public string last_trade_datetime { get; set; }
        public string matched { get; set; }
        public string bid_size { get; set; }
        public string net_pct_change { get; set; }
        public string updated { get; set; }
        public string end_mkt_day_price { get; set; }
        public string ask_price { get; set; }
        public string bid_price { get; set; }
        public string last { get; set; }
        public string pre_after_updated { get; set; }
        public string net_price_change { get; set; }
        public string pre_after_price { get; set; }
        public string net_change { get; set; }
    }

    public class Pre
    {
        public string after_percent_net_change { get; set; }
        public string after_net_change { get; set; }
    }

    public class Source
    {
        public Sungard sungard { get; set; }
        public Bats bats { get; set; }
        public Pre pre { get; set; }
    }

    public class Stock
    {
        public Source source { get; set; }
        public string exchange { get; set; }
        public string dividend_yield { get; set; }
        public string last { get; set; }
        public string ticker { get; set; }
        public string ticker_type { get; set; }
        public string zacks_rank_text { get; set; }
        public string volume { get; set; }
        public string updated { get; set; }
        public string percent_net_change { get; set; }
        public string zacks_rank { get; set; }
        public string name { get; set; }
        public string net_change { get; set; }
        public string market_time { get; set; }
        public string previous_close { get; set; }
        public string SUNGARD_BID { get; set; }
        public string SUNGARD_YRLOW { get; set; }
        public string SUNGARD_MARKET_CAP { get; set; }
        public string FEED_NET_CHANGE { get; set; }
        public string BATS_PRE_AFTER_UPDATED { get; set; }
        public string SUNGARD_EARNINGS { get; set; }
        public string SUNGARD_VOLATILITY { get; set; }
        public string SUNGARD_PE_RATIO { get; set; }
        public string SUNGARD_DAY_LOW { get; set; }
        public string SUNGARD_YRHIGH { get; set; }
        public string SUNGARD_DIVIDEND_FREQ { get; set; }
        public string SUNGARD_PREV_CLOSE_DATE { get; set; }
        public string BATS_ASK_PRICE { get; set; }
        public string SUNGARD_YIELD { get; set; }
        public string SUNGARD_DAY_HIGH { get; set; }
        public string SUNGARD_ZACKS_RECOMMENDATION { get; set; }
        public string SUNGARD_NAME { get; set; }
        public string SUNGARD_TIMESTAMP { get; set; }
        public string SUNGARD_VOLUME { get; set; }
        public string SUNGARD_BIDASKSIZE { get; set; }
        public string BATS_BID_PRICE { get; set; }
        public string BATS_LAST_TRADE_DATETIME { get; set; }
        public string FEED_VOLUME { get; set; }
        public string SUNGARD_SHARES { get; set; }
        public string SUNGARD_DIVIDEND { get; set; }
        public string SUNGARD_DIVIDEND_DATE { get; set; }
        public string BATS_BID_SIZE { get; set; }
        public string SUNGARD_LAST_TRADE_DATETIME { get; set; }
        public string SUNGARD_UPDATED { get; set; }
        public string BATS_ROUTED { get; set; }
        public string BATS_ASK_SIZE { get; set; }
        public string FEED_TICKER { get; set; }
        public string SUNGARD_POS_SIZE { get; set; }
        public string SUNGARD_EXCHANGE { get; set; }
        public string SUNGARD_TYPE { get; set; }
        public string BATS_PRE_AFTER_PRICE { get; set; }
        public string BATS_UPDATED { get; set; }
        public string FEED_LAST { get; set; }
        public string FEED_PERCENT_NET_CHANGE { get; set; }
        public string FEED_SOURCE { get; set; }
        public string FEED_UPDATED { get; set; }
        public string SUNGARD_OPEN { get; set; }
        public string SUNGARD_CLOSE { get; set; }
        public string SUNGARD_ASK { get; set; }
        public string BATS_MATCHED { get; set; }
        public string pre_after_net_change { get; set; }
        public string pre_after_percent_net_change { get; set; }
        public string ticker_market_status { get; set; }
        public string ap_short_name { get; set; }
        public string market_status { get; set; }
        public string previous_close_date { get; set; }
    }
}
