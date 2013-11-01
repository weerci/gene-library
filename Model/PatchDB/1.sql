select prf as profile_id,
			t.kind_id,
		 case when t.kind_id = 1 then (select count(*) from modern.chk_allele where profile_id = 33219) -  else
       	
		 (select count(*) from modern.chk_allele where profile_id = 33219) - sum(chk) cnt,
     (select card_num from modern.card where id = prf) card_num,
     (select exam_num from modern.card where id = prf) exam_num
from (select a.profile_id as prf,
             locus_id,
             c.kind_id,
             case when a.allele_id in (select allele_id  from modern.chk_allele where profile_id = 33219) then 1 else 0 end chk
        from modern.chk_allele a inner join modern.card c on a.profile_id = c.id
       where a.profile_id <> 33219) t
group by prf, kind_id
having((select count(*) from modern.chk_allele where profile_id = 33219) - sum(chk) < 2) and modern.locus_cnt(33219, prf) > 8
