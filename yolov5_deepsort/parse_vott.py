import csv

for i in range(100):
    current_file = str(i + 1).zfill(8)
    with open('data_vott.csv', 'a', encoding='UTF8', newline='') as f:
        csv_writer = csv.writer(f, quoting=csv.QUOTE_NONNUMERIC)
        with open("labels/"+ current_file + ".csv") as csv_file:
            csv_reader = csv.reader(csv_file, delimiter=',')
            line_cnt = 0
            
            for row in csv_reader:
                if line_cnt != 0:
                    xmin = row[0]
                    ymin = row[1]
                    xmax = row[2]
                    ymax = row[3]
                    
                    data = [current_file + ".png", int(xmin), int(ymin), int(xmax), int(ymax), "zebrafish"]
                    csv_writer.writerow(data)
                line_cnt += 1
