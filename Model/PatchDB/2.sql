select t.profile_id --, sum(t_ba) - sum(cnt_coin) cnt_coin
  from (select ch.profile_id,
               ch.locus_id ,
               --count(distinct ch.allele_id) t_ca,
               count(distinct ch1.allele_id) t_ba,
               sum(case when ch.allele_id = ch1.allele_id then 1 else 0 end) cnt_coin
          from modern.chk_allele ch
         inner join modern.chk_allele ch1
            on ch1.locus_id = ch.locus_id
           and ch1.profile_id = 33219
         where ch.profile_id <> 33219 -- where ch.profile_id = 4
         group by ch.profile_id, ch.locus_id) t
 group by t.profile_id
having sum(t_ba) - sum(cnt_coin) < 2 and count(*) > 8
